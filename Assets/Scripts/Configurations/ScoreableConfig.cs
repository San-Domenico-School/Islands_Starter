/*******************************************************************
* This is attached to each Scoreable Prefab
*
* It holds the score of the attached prefab which is read by the 
* player on collision.
*
* Bruce Gustin
* Jan 6, 2026
*******************************************************************/

using UnityEngine;

public class ScoreableConfig : MonoBehaviour
{
    [SerializeField] private Color effectColor;
    public int scoreValue;

    void Start()
    {
        ParticleSystemRenderer psRenderer = GetComponent<ParticleSystemRenderer>();

        if (psRenderer != null)
        {
            // This changes the "_Color" property on the material
            psRenderer.material.color = effectColor;
        }
    }
}

