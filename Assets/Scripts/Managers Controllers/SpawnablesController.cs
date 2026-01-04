/*******************************************************************
* This is attached to each Collectable Prefab
*
* It determines how long that collectable remains in the scene
* and provides rotation animation capabilities
*
* Bruce Gustin
* Jan 2, 2026
*******************************************************************/

using System.Collections;
using UnityEngine;

public class SpawnablesController : MonoBehaviour
{
    [Range(3, 30)]
    [SerializeField] private float timeInScene;

    [Tooltip("Use 0.00 to 1.0 in each axis")]
    [SerializeField] private Vector3 rotation;
    
    // Start the Coroutine at initialization
    void Start()
    {
        StartCoroutine("RemoveObjectFromScene");
    }

    // Gives the object a rotation animation
    void Update()
    {
        transform.Rotate(rotation);
    }

    //Destroys the game object after the alloted time as passed.
    IEnumerator RemoveObjectFromScene()
    {
        yield return new WaitForSeconds(timeInScene);
        Destroy(gameObject);
    }
}

 
