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
using UnityEngine.InputSystem.Controls;

public class PlayerJoinHandler : MonoBehaviour
{
    [Tooltip("Drag the Player Manager GameObject into this field from the Hierarchy")]
    [SerializeField] private PlayerInputManager playerInputManager;
    
    // Define colors for each button
    [SerializeField] private Color colorA = Color.red;
    [SerializeField] private Color colorB = Color.blue;
    [SerializeField] private Color colorX = Color.green;
    [SerializeField] private Color colorY = Color.yellow;
    [SerializeField] private Color colorKeyboard = Color.white;

    void Update()
    {
        // Check keyboard input first
        CheckKeyboardJoin();
        
        // Check all connected gamepads
        CheckGamepadJoin();
    }

    // The Keyboard Join is only used for testing purposes
    void CheckKeyboardJoin()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // You can choose which key joins for keyboard players
        if (keyboard.spaceKey.wasPressedThisFrame || keyboard.enterKey.wasPressedThisFrame)
        {
            PlayerInput newPlayer = playerInputManager.JoinPlayer(pairWithDevice: keyboard);
            if (newPlayer != null)
            {
                SetPlayerColor(newPlayer, colorKeyboard);
                SetTeamID(newPlayer, 0);
            }
        }
    }

    // Cycles through each of the Gamepad Buttons and determines if a specific button was pressed
    // If A, B, X, Y were pressed, it assign the color and ID.
    void CheckGamepadJoin()
    {
        // Loop through all gamepads to allow multiple controllers to join
        foreach (var gamepad in Gamepad.all)
        {
            // These build 2 nullible wrappers 
            Color? selectedColor = null;
            int? teamID = null;

            if (gamepad.buttonSouth.wasPressedThisFrame) 
            {
                // A button
                selectedColor = colorA;
                teamID = 0;
            }
            else if (gamepad.buttonEast.wasPressedThisFrame)
            {
                 // B button
                selectedColor = colorB;
                teamID = 1;
            }
            else if (gamepad.buttonWest.wasPressedThisFrame)
            {
                // X button
                selectedColor = colorX;
                teamID = 2;
            }
            else if (gamepad.buttonNorth.wasPressedThisFrame)
            {
                 // Y button
                selectedColor = colorY;
                teamID = 3;
            }
            if (selectedColor.HasValue)
            {
                PlayerInput newPlayer = playerInputManager.JoinPlayer(pairWithDevice: gamepad);
                if (newPlayer != null)
                {
                    SetPlayerColor(newPlayer, selectedColor.Value);
                    SetTeamID(newPlayer, teamID.Value);
                }
            }
        }
    }

    // This changes the Player's color to distinquish it during game play
    void SetPlayerColor(PlayerInput playerInput, Color color)
    {
        // Assuming your player has a Renderer component
        var renderer = playerInput.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
        
        // Or if you're using a SpriteRenderer
        var spriteRenderer = playerInput.GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }

    // This assigns the TeamID in the PlayerController based on the button pressed,
    // 0 for 9th grade, 1 for 10th grade, 2 for 11th grade, 3 for 12th grade,
    void SetTeamID(PlayerInput playerInput, int teamID)
    {
        var  playerScoreCollector = playerInput.GetComponentInChildren< PlayerScoreCollector>();
        if ( playerScoreCollector != null)
        {
            playerScoreCollector.teamID = teamID;
        }
    }
}
