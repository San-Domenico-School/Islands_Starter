/*******************************************************************
* This is attached to the Player
*
* Detects collisions with power-up items and manages their lifecycle.
* This script uses a C# Event system (Observer Pattern) to notify 
* other components when a power-up starts and ends, while providing
* a central visual indicator through the Player's Light component.
* 
* Bruce Gustin
* Jan 2, 2026
*******************************************************************/

using System;
using System.Collections;
using UnityEngine;

public class PlayerPowerupHandler : MonoBehaviour
{
    // These actions act as "Radio Stations" that students can tune into
    public static event Action<PowerUpData> OnPowerUpApplied;
    public static event Action<PowerUpData> OnPowerUpExpired;

    private bool hasPowerup;
    private Light lightComponent;


    // Caches the Light component used for visual feedback.
    void Start()
    {
        lightComponent = GetComponent<Light>();
    }

    // Detects contact with power-ups. Using the 'hasPowerup' flag, it ensures only one power-up effect
    // can be active at a time. New pickups deleted but not initiated.
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            if(!hasPowerup)
            {
                PowerUpConfig config = other.GetComponent<PowerUpConfig>();
                if (config != null && config.data != null)
                {
                    StartCoroutine(PowerUpRoutine(config.data));
                }
            }
            Destroy(other.gameObject);
        }
    }

    // Manages the timing of the power-up. Updates the player's light color,  notifies listeners 
    // of the start, waits for the duration, and then notifies listeners to clean up.
    IEnumerator PowerUpRoutine(PowerUpData data)
    {
        hasPowerup = true;
        // 1. Visual Indicator (Teacher's shared code)
        if (lightComponent) 
        { 
            lightComponent.color = data.colorIndicator; 
            lightComponent.intensity = 8; 
        }

        // 2. Broadcast to all student scripts: "A powerup has started!"
        OnPowerUpApplied?.Invoke(data);

        yield return new WaitForSeconds(data.duration);

        // 3. Broadcast to all student scripts: "It's over, clean up!"
        OnPowerUpExpired?.Invoke(data);

        // 4. Reset Visuals
        if (lightComponent) lightComponent.intensity = 0;
        hasPowerup = false;
    }
}