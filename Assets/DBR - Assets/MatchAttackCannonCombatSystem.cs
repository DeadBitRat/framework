using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchAttackCannonCombatSystem : MonoBehaviour
{

    public GameObject hitBox; 
    public MathAttackHealthSystem healthSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroyEnemyOnTouch(GameObject enemy)
    {
        Destroy(enemy); 
    }

    public void CannonBeingTouched(GameObject enemy)
    {
        DestroyEnemyOnTouch(enemy);
        healthSystem.recieveDamage(1); 
    }
}
