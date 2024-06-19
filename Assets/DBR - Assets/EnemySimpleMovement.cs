using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform tr;

    public float xSpeed; 
    public float ySpeed;
    public string  xDirection; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xDirection == "Right")
        {
            rb.velocity = new Vector2(xSpeed, ySpeed);
            tr.localScale = new Vector2(1f, 1f);

        }

        else if (xDirection == "Left")
        {
            rb.velocity = new Vector2(-xSpeed, ySpeed);
            tr.localScale = new Vector2(-1f, 1f);
        }

        else

        {
            Debug.LogWarning("Enemy Simple Movement needs xDirection");
        }
    }
}
