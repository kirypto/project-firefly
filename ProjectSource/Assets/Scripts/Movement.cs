using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //MOvement
    private float xMove;
    private float jMove;
    public float speed;
    public float jumpHeight;

    public float jumpCool;
    private float jumpTime;
    public Rigidbody2D rb;

    private bool allowJump;
    // Start is called before the first frame update
    void Awake()
    {
        xMove = 0.0f;
        jMove = 0.0f;
        
        speed = 8.0f;
        jumpHeight = 5.0f;

        jumpCool = 0.0f;
        jumpTime = 5.0f;
        rb = GetComponent<Rigidbody2D>();
        allowJump = true;
    }

    // Update is called once per frame
    void FixedUpdate(){
    
        xMove = Input.GetAxis("Horizontal");
        Vector2 Move_Horizontal = new Vector2(xMove, 0.0f);
        rb.AddForce(Move_Horizontal * speed);

            jMove = Input.GetAxis("Jump");
        if (jMove > 0 && allowJump)
        {
            allowJump = false;
            Invoke(nameof(ResetJumpCooldown), 1.5f);
            Vector2 Move_Jump = new Vector2(0.0f, jMove);
            rb.AddForce((Move_Jump * jumpHeight), ForceMode2D.Impulse);
            jumpCool = jumpTime;
        }

    }

    private void ResetJumpCooldown()
    {
        allowJump = true;
    }
    
        
}
