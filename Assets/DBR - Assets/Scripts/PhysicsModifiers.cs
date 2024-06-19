using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhysicsModifiers : MonoBehaviour
{
    public GameplaySystem gameplaySystem;
    private Rigidbody2D baseRB; 

    public HorizontalMovement hm;

    public CapsuleCollider2D capsuleCollider; 

    // Start is called before the first frame update
    void Start()
    {
        baseRB = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.Platformer)
        {
            SetCapsuleColliderSize(new Vector2(0.16f, 0.24f), new Vector2(0f, 0.12f), CapsuleDirection2D.Vertical);
        }


        if (gameplaySystem.gameplay == GameplaySystem.GameplayType.TopDown)
        {
            SetCapsuleColliderSize(new Vector2(0.16f, 0.06f), new Vector2(0f,0f), CapsuleDirection2D.Horizontal);
        }
    }

    public void HorizontalSpeed(float speed)
    {
        hm.hSpeedModifier = speed;
    }

    private void SetCapsuleColliderSize(Vector2 size, Vector2 offset, CapsuleDirection2D direction)
    {
        capsuleCollider.size = size;
        capsuleCollider.offset = offset;
        capsuleCollider.direction = direction;
    }


}
