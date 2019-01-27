using UnityEngine;

public class ArmMovementScript : MonoBehaviour
{
    private bool _isSwingingForward;
    private float _timeStartedLerping;
    private Quaternion _startRotation;
    private Quaternion _endRotation;
    private bool _isSwingingReturn;

    private void Update()
    {
        if (0 < Input.GetAxis("Fire3") && !_isSwingingForward && !_isSwingingReturn)
        {
            SwingArm();
        }
    }

    private void FixedUpdate()
    {
        if(_isSwingingForward)
        {
            HandleForwardSwing();
        } else if (_isSwingingReturn)
        {
            HandleReturnSwing();
        }
    }

    private void HandleReturnSwing()
    {
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / 1f;

        transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, percentageComplete);
        if (percentageComplete >= 1.0f)
        {
            _isSwingingReturn = false;
        }
    }

    private void HandleForwardSwing()
    {
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / 1f;

        transform.rotation = Quaternion.Lerp(_startRotation, _endRotation, CalculateLerpVal(percentageComplete));

        if (percentageComplete >= 1.0f)
        {
            _isSwingingForward = false;
            _isSwingingReturn = true;
            _timeStartedLerping = Time.time;

            _startRotation = transform.rotation;
            _endRotation = Quaternion.AngleAxis(-205f, Vector3.forward);
        }
    }


    private void SwingArm()
    {
        _isSwingingForward = true;
        _timeStartedLerping = Time.time;
 
        _startRotation = transform.rotation;
        _endRotation = Quaternion.AngleAxis(-90f, Vector3.forward);
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