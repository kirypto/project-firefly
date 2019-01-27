using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float inertiaOvercomeScalar = 1f;
    [SerializeField] private float inertiaOvercomeThreshold = 1f;

    //Movement
    private float _xMove;
    private float _jMove;
    public float speed = 75f;
    public float jumpHeight = 20f;

    public Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;

    private bool _allowJump;
    private ArmMovementScript _armMovementScript;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _allowJump = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _armMovementScript = GameObject.Find("arm").GetComponent<ArmMovementScript>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        _xMove = Input.GetAxis("Horizontal");
        if (_xMove > 0f || _xMove < 0f)
        {
            Vector2 moveHorizontal = new Vector2(_xMove, 0.0f);
            float velocityMag = Mathf.Abs(rb.velocity.magnitude);
            if (velocityMag < inertiaOvercomeThreshold)
            {
                moveHorizontal *= inertiaOvercomeScalar;
            }
            rb.AddForce(moveHorizontal * speed);
        }

        _jMove = Input.GetAxis("Jump");
        if (_jMove > 0 && _allowJump)
        {
            _allowJump = false;
            Invoke(nameof(ResetJumpCooldown), 1.5f);
            Vector2 moveJump = new Vector2(0.0f, _jMove);
            rb.AddForce((moveJump * jumpHeight), ForceMode2D.Impulse);
        }

        if (_xMove > 0.05f)
        {
            spriteRenderer.flipX = false;
            _armMovementScript.Direction = FacingDirection.Right;
        } else if (_xMove < -0.05f)
        {
            spriteRenderer.flipX = true;
            _armMovementScript.Direction = FacingDirection.Left;
        }
    }

    private void ResetJumpCooldown()
    {
        _allowJump = true;
    }
}