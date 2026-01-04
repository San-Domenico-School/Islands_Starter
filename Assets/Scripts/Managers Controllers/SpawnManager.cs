/**********************************************************************
* This script is attached to the Spawn Manager
*
* It spawns collectables into the scene.  
* 
* Bruce Gustin
* Jan 4, 2026
**********************************************************************/

using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Objects to Spawn")]
    [SerializeField] private GameObject[] powerups;
     [SerializeField] private GameObject scoreable;
    [SerializeField] private GameObject enemy;
   
    [Header("Spawn Rates per Minute")]
    [Range(3, 30)]
    [SerializeField] private int powerupRate;   
        
    [Range(3, 30)]
    [SerializeField] private int scoreableRate;   

    [Header("Maximum Enemies Per Wave")]
    [SerializeField] private float maxWave;
    private int currentWave;

    [HideInInspector]
    public static int enemyCount;


    // Spawns called per fram
    void Update()
    {
        SpawnPowerup();
        SpawnScoreable();
        SpawnEnemy();
    }
    
    // Spawns a specific powerup at a specific position at powerupRate
    void SpawnPowerup()
    {
        float spawnThreshold = powerupRate / 60f * Time.deltaTime;
        if(Random.value < spawnThreshold)
        {
            int choiceIndex = Random.Range(0, powerups.Length); 
            Vector3 position = SpawnLocation();
            Instantiate(powerups[choiceIndex], position, transform.rotation); 
        }
    }
    
    // Spawns a specific scoreable at a specific position at scoreableRate
    void SpawnScoreable()
    { 
        float spawnThreshold = scoreableRate / 60f * Time.deltaTime;
        if(Random.value < spawnThreshold)
        {
            Vector3 position = SpawnLocation();
            Instantiate(scoreable, position, transform.rotation); 
        }
    }

    // Spawns a wave of enemies when there are no enemies present.
    // Wave grows each cycle up to the maximum wave
    void SpawnEnemy()
    {
        if(enemyCount == 0)
        {
            // Spawn a wave of enemies
            for(int i = 0; i < currentWave; i++)
            {
                Vector3 position = SpawnLocation();
                Instantiate(enemy, position, transform.rotation);
                enemyCount++;
            }
            
            // Set wave size, limited to max wave
            if (currentWave < maxWave) 
            {
                currentWave++;
            }
        }
    }

    private Vector3 SpawnLocation()
    {
        Vector3 position = Vector3.zero;

        while (position.magnitude == 0 || position.magnitude > 12.3)
        {
            float xPos = Random.Range(-11.0f, 11.0f); 
            float zPos = Random.Range(-11.0f, 11.0f); 
            position = new Vector3(xPos, 0, zPos);
        }
        return position;
    }
}
