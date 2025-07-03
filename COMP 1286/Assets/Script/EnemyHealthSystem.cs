using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public int initialHitPoints = 2;
    private int hitPointsLeft;
    private SpriteRenderer sr;
    private Color initialSpriteColour;
    public Color deathColour = Color.red;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initialSpriteColour = sr.color;
        hitPointsLeft = initialHitPoints;
    }

    public void ReceiveHit(int damage)
    {
        hitPointsLeft = hitPointsLeft - damage;
        ChangeColour();

        if (hitPointsLeft <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void ChangeColour()
    {
        float percentageOfDamageTaken = 1f - ((float)hitPointsLeft/(float)initialHitPoints);
        Color newHealthColour = Color.Lerp(initialSpriteColour, deathColour, percentageOfDamageTaken);
        sr.color = newHealthColour;
    }
}
