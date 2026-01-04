/*******************************************************************
* This is attached to each Enemy Prefab
*
* Manages enemy behavior including identifying and pursuing the 
* nearest player using a distance-based search. This script also 
* implements a "melting" mechanic that gradually reduces the 
* object's scale and mass over time, handling its destruction 
* once it reaches a minimum volume threshold or falls off the map.
*
* Bruce Gustin
* Jan 4, 2026
*******************************************************************/

using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1.55f;
    
    private Transform targetPlayer;
     private float reductionEachRepeat;
    private float minimumVolume;

    private Rigidbody iceRB;
    private ParticleSystem iceVFX;

    // Initializes components, randomizes initial physical stats, 
    // identifies the initial target, and begins the melting cycle.
    void Start()
    {
        reductionEachRepeat = .985f;
        minimumVolume = .15f;
        iceRB = GetComponent<Rigidbody>();
        iceVFX = GetComponent<ParticleSystem>();

        RandomizeSizeAndMass();
        FindNearestPlayer();
        InvokeRepeating("Melt", 1, 0.5f);
    }

    // Executes movement logic toward the assigned target every frame.
    void Update()
    {
       FollowNearestPlayer();
    }

    // Scans the scene for all Players at the moment of spawning. Identifies and
    // stores the closest player as the permanent target for this enemy's lifespan.
    void FindNearestPlayer()
    {
        PlayerMovement[] players = FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (PlayerMovement playerComponent in players)
        {
            GameObject player = playerComponent.gameObject;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        targetPlayer = closestPlayer;
    }

    // Translates the enemy position toward the targetPlayer's current 
    // position based on moveSpeed.
    private void FollowNearestPlayer()
    {
         if (targetPlayer != null)
        {
            // Calculate the step based on speed and frame time
            float step = moveSpeed * Time.deltaTime;
            
            // Move our position a step closer to the target
            transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, step);
        }
    }
    // Applies a random scale reduction to the enemy on spawn to 
    // vary enemy sizes and adjusts Rigidbody mass proportionally.
       private void RandomizeSizeAndMass()
    {
        float sizeReduction = Random.Range(0.4f, .75f);
        transform.localScale *= sizeReduction;
        iceRB.mass *= sizeReduction;
    }

    // Calculates the current volume of the enemy. Destroys the object and updates the
    // SpawnManager count if the enemy is too small or has fallen below the world threshold
    private void Dissolution()
    {
        float volume = 4f / 3f * Mathf.PI * Mathf.Pow(transform.localScale.x, 3);
        if (volume < minimumVolume || transform.position.y < -10)
        {
            iceVFX.Stop();
            SpawnManager.enemyCount--;
            Destroy(gameObject);
        }
    }

    // Periodically reduces the scale and mass of the enemy to 
    // simulate melting, then triggers the dissolution check.
    private void Melt()
    {
        transform.localScale *= reductionEachRepeat;
        iceRB.mass *= reductionEachRepeat;
        Dissolution();
    }
}