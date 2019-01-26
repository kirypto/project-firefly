using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //Movement
    private float _xMove;
    private float _jMove;
    public float speed;
    public float jumpHeight;

    public float jumpCool;
    private float _jumpTime;
    public Rigidbody2D rb;

    private bool _allowJump;

    // Start is called before the first frame update
    private void Awake()
    {
        _xMove = 0.0f;
        _jMove = 0.0f;

        speed = 8.0f;
        jumpHeight = 5.0f;

        jumpCool = 0.0f;
        _jumpTime = 5.0f;
        rb = GetComponent<Rigidbody2D>();
        _allowJump = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _xMove = Input.GetAxis("Horizontal");
        Vector2 moveHorizontal = new Vector2(_xMove, 0.0f);
        rb.AddForce(moveHorizontal * speed);
        _jMove = Input.GetAxis("Jump");

        if (_jMove > 0 && _allowJump)
        {
            _allowJump = false;
            Invoke(nameof(ResetJumpCooldown), 1.5f);
            Vector2 moveJump = new Vector2(0.0f, _jMove);
            rb.AddForce((moveJump * jumpHeight), ForceMode2D.Impulse);
            jumpCool = _jumpTime;
        }
    }

    private void ResetJumpCooldown()
    {
        _allowJump = true;
    }
}