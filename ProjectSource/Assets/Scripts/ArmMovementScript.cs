using UnityEngine;

public class ArmMovementScript : MonoBehaviour
{
    [SerializeField] private bool debugMode;
    [SerializeField] private float facingLeftStartAngle;
    [SerializeField] private float facingLeftEndAngle;
    [SerializeField] private float facingRightStartAngle;
    [SerializeField] private float facingRightEndAngle;

    private bool _isSwingingForward;
    private float _timeStartedLerping;
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private bool _isSwingingReturn;
    private bool _isFacingLeft = true;

    private void Update()
    {
        if (0 < Input.GetAxis("Fire3") && !_isSwingingForward && !_isSwingingReturn)
        {
            SwingArm();
        }

        // ------------------------- DEBUG ----------------------
        _isFacingLeft = !debugMode;
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

            _startRotation = transform.rotation;
            _endRotation = Quaternion.AngleAxis(-205f, transform.forward);
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

    private float SwingStartAngle => _isFacingLeft ? facingLeftStartAngle : facingRightStartAngle;
    private float SwingEndAngle => _isFacingLeft ? facingLeftEndAngle : facingRightEndAngle;

    private Quaternion SwingStartRotation => Quaternion.AngleAxis(SwingStartAngle, Vector3.forward);
    private Quaternion SwingEndRotation => Quaternion.AngleAxis(SwingEndAngle, Vector3.forward);

    private void SwingArm()
    {
        _isSwingingForward = true;
        _timeStartedLerping = Time.time;

        _startRotation = transform.rotation;
        _endRotation = Quaternion.AngleAxis(-90f, transform.forward);
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

        Debug.Log("CAUGHT SOMETHING");
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCameraScript>().MarkFireflyCaught();
        Destroy(other.gameObject);
    }
}