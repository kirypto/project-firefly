using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float inertiaOvercomeScalar = 1f;
    [SerializeField] private float inertiaOvercomeThreshold = 1f;
    [SerializeField] private bool debugMode;

    //Movement
    private float _xMove;
    private float _jMove;
    public float speed = 75f;
    public float jumpHeight = 20f;
    private float distToGround;

    public Rigidbody2D rb;

    private SpriteRenderer _spriteRenderer;

    private ArmMovementScript _armMovementScript;

    public bool IsMovementAllowed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _armMovementScript = GameObject.Find("arm").GetComponent<ArmMovementScript>();
        IsMovementAllowed = false;
        distToGround = GetComponent<Collider2D>().bounds.extents.y;

        // -------------------------- DEBUG ----------------------------
        if (debugMode)
        {
            IsMovementAllowed = true;
        }
    }

    private void Update()
    {
        if (!IsMovementAllowed)
        {
            return;
        }

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
        if (_jMove > 0 && IsGrounded() && rb.velocity.y == 0)
        {
            Vector2 moveJump = new Vector2(0.0f, _jMove);
            print("Jumping");
            rb.AddForce((moveJump * jumpHeight), ForceMode2D.Impulse);
        }

        if (_xMove > 0.05f)
        {
            _spriteRenderer.flipX = false;
            _armMovementScript.Direction = FacingDirection.Right;
        }
        else if (_xMove < -0.05f)
        {
            _spriteRenderer.flipX = true;
            _armMovementScript.Direction = FacingDirection.Left;
        }
    }

    public  bool IsGrounded() {
        return Physics2D.Raycast(transform.position, -Vector2.up, distToGround + 0.1f, LayerMask.GetMask("Background"));
    }

    public void SpawnAtLocation(Vector2 location)
        {
            transform.position = location;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
        }
}