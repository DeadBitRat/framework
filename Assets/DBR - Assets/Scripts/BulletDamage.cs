using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDamage : MonoBehaviour
{
    [HideInInspector]
    public Collider2D col;
    [HideInInspector]
    public Rigidbody2D rb;

    public float damage;

    private Selfdestroy destroy;

    public bool enemyBullet;
    public bool playerBullet;

    public bool friendlyDamage; 
    public bool enemyFriendlyDamage;

    public float damageDuration = 1f; 


    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>(); rb = GetComponent<Rigidbody2D>();
        destroy = GetComponent<Selfdestroy>();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        
        if (collision.gameObject.GetComponent<HealthSystem>() != null)
        {
            collision.gameObject.GetComponent<HealthSystem>().ReceivingDamage(damage, damageDuration); 
            
        }

        destroy.Destroy();

    }

}
