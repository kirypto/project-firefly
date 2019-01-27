using UnityEngine;
using UnityEngine.Serialization;

public class ArmMovementScript : MonoBehaviour
{
    [SerializeField] private float facingLeftStartAngle;
    [SerializeField] private float facingLeftEndAngle;
    [SerializeField] private float facingRightStartAngle;
    [SerializeField] private float facingRightEndAngle;

    private bool _isSwingingForward;
    private float _timeStartedLerping;
    private bool _isSwingingReturn;
    private SpriteRenderer _spriteRenderer;
    public FacingDirection Direction { get; set; } = FacingDirection.Left;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (0 < Input.GetAxis("Fire3") && !_isSwingingForward && !_isSwingingReturn)
        {
            SwingArm();
        }

        SpriteRenderer parent_renderer = transform.parent.GetComponent<SpriteRenderer>();
       _spriteRenderer.sortingOrder = Direction == FacingDirection.Right
                ? Mathf.Abs(parent_renderer.sortingOrder + 1)
                : Mathf.Abs(parent_renderer.sortingOrder - 1);

       if (Direction == FacingDirection.Right)
       {
           _spriteRenderer.flipX = false;
       }
       else
       {
           _spriteRenderer.flipX = true;
       }
    }

    private void FixedUpdate()
    {
        if (_isSwingingForward)
        {
            HandleForwardSwing();
        }
        else if (_isSwingingReturn)
        {
            HandleReturnSwing();
        }
        else
        {
            transform.rotation = SwingStartRotation;
        }
    }

    private void HandleForwardSwing()
    {
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / 1f;

        transform.rotation = Quaternion.Lerp(SwingStartRotation, SwingEndRotation, CalculateLerpVal(percentageComplete));

        if (percentageComplete >= 1.0f)
        {
            _isSwingingForward = false;
            _isSwingingReturn = true;
            _timeStartedLerping = Time.time;

            Quaternion.AngleAxis(-205f, transform.forward);
        }
    }

    private void HandleReturnSwing()
    {
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / 1f;

        transform.rotation = Quaternion.Lerp(SwingEndRotation, SwingStartRotation, percentageComplete);
        if (percentageComplete >= 1.0f)
        {
            _isSwingingReturn = false;
        }
    }

    private float SwingStartAngle => Direction == FacingDirection.Left ? facingLeftStartAngle : facingRightStartAngle;
    private float SwingEndAngle => Direction == FacingDirection.Left ? facingLeftEndAngle : facingRightEndAngle;

    private Quaternion SwingStartRotation => Quaternion.AngleAxis(SwingStartAngle, Vector3.forward);
    private Quaternion SwingEndRotation => Quaternion.AngleAxis(SwingEndAngle, Vector3.forward);

    private void SwingArm()
    {
        _isSwingingForward = true;
        _timeStartedLerping = Time.time;
        transform.parent.GetComponent<AudioSourceController>().Play();

        Quaternion.AngleAxis(-90f, transform.forward);
    }

    private static float CalculateLerpVal(float x)
    {
        // Black Magic
        float e = 2.718281828459045235360287471352662497757247093699959574966f;
        return (Mathf.Pow(e, ((x - 0.25f) * 15f)) / (Mathf.Pow(e, ((x - 0.25f) * 15f)) + 1f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Equals("Firefly"))
        {
            return;
        }

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCameraScript>().MarkFireflyCaught();
        Destroy(other.gameObject);
    }
}