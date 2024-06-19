using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Collider2D col;
    private Rigidbody2D rb;
    private Transform tr;  

    public float xbulletSpeed;
    public float ybulletSpeed;

    public string xDirection; 

    public GameObject target; 

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>(); rb = GetComponent<Rigidbody2D>(); tr = GetComponent<Transform>();
       

    }

    // Update is called once per frame
    void Update()
    {
        if (xDirection == "Right")
        {
            rb.velocity = new Vector2(xbulletSpeed, ybulletSpeed);
            tr.localScale = new Vector2(1f,1f);

        }

        else if (xDirection == "Left")
        {
            rb.velocity = new Vector2(-xbulletSpeed, ybulletSpeed);
            tr.localScale = new Vector2(-1f, 1f);
        }
    }


}
