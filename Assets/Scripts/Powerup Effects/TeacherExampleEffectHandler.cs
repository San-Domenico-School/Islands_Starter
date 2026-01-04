/**********************************************************************
* This script is an Effect Handler attached to a Player child object.
*
* This component listens for global Power-Up events dispatched by the 
* PlayerPowerupHandler. It acts as a specialized "listener" that 
* modifies specific player attributes (e.g., speed, physics, scoring) 
* only when relevant PowerUpData is received.
* 
* This modular approach allows for adding new power-up behaviors 
* without modifying existing player movement or combat code which 
* would cause merge errors
*
* This teacher example is one that has a visible effect, but is not 
* useful in the game.  It is for demonstration purposes only.
* 
* Bruce Gustin
* Jan 2, 2026
**********************************************************************/

using System;
using System.Collections;
using UnityEngine;
public class TeacherExampleEffectHandler : MonoBehaviour
{
private Vector3 originalScale;

    void OnEnable() 
    {
        // Subscribe to the global Power-Up events
        PlayerPowerupHandler.OnPowerUpApplied += StretchPlayer;
        PlayerPowerupHandler.OnPowerUpExpired += ResetScale;
    }

    void OnDisable() 
    {
        // Unsubscribe to prevent errors when this object is destroyed
        PlayerPowerupHandler.OnPowerUpApplied -= StretchPlayer;
        PlayerPowerupHandler.OnPowerUpExpired -= ResetScale;
    }

    private void StretchPlayer(PowerUpData data) 
    {
        // We only trigger this if the power-up has a specific "fun" color
        // This prevents the player from stretching for EVERY power-up
        if (data.powerUpName.Equals("Teacher Example"))
        {
            originalScale = transform.parent.localScale;
            // Stretch the player vertically like a noodle
            transform.parent.localScale = new Vector3(0.75f, .75f, 0.75f);
            Debug.Log("Power-Up Applied: I'm a noodle!");
        }
    }

    private void ResetScale(PowerUpData data) 
    {
        if (data.powerUpName.Equals("Teacher Example"))
        {
            // Return the player to their original size
            transform.parent.localScale = originalScale;
            Debug.Log("Power-Up Expired: Back to normal size.");
        }
    }
}
