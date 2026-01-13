/*******************************************************************
* This script is attached to the Player Manager Empty
* Its purpose is to manage player entry into the game by detecting 
* inputs from keyboards or gamepads, pairing them with the 
* PlayerInputManager, and initializing the player's color and 
* team ID based on the specific button pressed during join.
*
* Bruce Gustin
* Jan 2, 2026
*******************************************************************/

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic; // Required for HashSet

public class PlayerJoinHandler : MonoBehaviour
{
    [Tooltip("Drag the Player Manager GameObject into this field from the Hierarchy")]
    [SerializeField] private PlayerInputManager playerInputManager;
    
    [SerializeField] private Color colorA;
    [SerializeField] private Color colorB;
    [SerializeField] private Color colorX;
    [SerializeField] private Color colorY;
    [SerializeField] private Color colorKeyboard;
    [SerializeField] private int playersPerTeam = 1;

    // Track which devices have already joined
    private HashSet<InputDevice> joinedDevices = new HashSet<InputDevice>();
    private int[] teamSelected = new int[4];

    void Start()
    {
        GlobalEvents.PlayersPerTeam = playersPerTeam;
    }
    
    void Update()
    {
        CheckKeyboardJoin();
        CheckGamepadJoin();
    }

    void CheckKeyboardJoin()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null || joinedDevices.Contains(keyboard)) return;

        if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame)
        {
            Join(keyboard, colorKeyboard, 0);
        }
    }

    void CheckGamepadJoin()
    {
        foreach (var gamepad in Gamepad.all)
        {
            // SKIP if this specific controller is already playing
            if (joinedDevices.Contains(gamepad)) continue;

            // Set color and int to nullable
            Color? selectedColor = null;
            int? teamID = null;

            if (gamepad.buttonEast.wasPressedThisFrame && teamSelected[0] < GlobalEvents.PlayersPerTeam) 
            {
                selectedColor = colorA;
                teamID = 0;
            }
            else if (gamepad.buttonSouth.wasPressedThisFrame && teamSelected[1] < GlobalEvents.PlayersPerTeam)
            {
                selectedColor = colorB;
                teamID = 1;
            }
            else if (gamepad.buttonNorth.wasPressedThisFrame && teamSelected[2] < GlobalEvents.PlayersPerTeam)
            {
                selectedColor = colorX;
                teamID = 2;
            }
            else if (gamepad.buttonWest.wasPressedThisFrame && teamSelected[3] < GlobalEvents.PlayersPerTeam)
            {
                selectedColor = colorY;
                teamID = 3;
            }

            if (selectedColor.HasValue)
            {
                Join(gamepad, selectedColor.Value, teamID.Value);
            }
        }
    }

    // Refactored Join logic to avoid repetition
    void Join(InputDevice device, Color color, int teamID)
    {
        PlayerInput newPlayer = playerInputManager.JoinPlayer(pairWithDevice: device);
        if (newPlayer != null)
        {
            joinedDevices.Add(device); // Mark this controller as "used"
            teamSelected[teamID]++;
            SetPlayerColor(newPlayer, color);
            SetTeamID(newPlayer, teamID);
        }
    }

    void SetPlayerColor(PlayerInput playerInput, Color color)
    {
        var renderer = playerInput.GetComponentInChildren<Renderer>();
        if (renderer != null) renderer.material.color = color;
        
        var spriteRenderer = playerInput.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null) spriteRenderer.color = color;
    }

    void SetTeamID(PlayerInput playerInput, int teamID)
    {
        var playerScoreHandler = playerInput.GetComponentInChildren<PlayerScoreHandler>();
        if (playerScoreHandler != null)
        {
            playerScoreHandler.teamID = teamID;
        }
    }
}