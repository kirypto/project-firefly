using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovementScript : MonoBehaviour
{
    private float _armMove = 2.0f;
    private bool _allowSwing = true;
    private float _coolTime = 2.0f;
    
    private Rigidbody2D _rb;
    // Start is called before the first frame update

    // Update is called once per frame
    private void Update()
    {
        
       _armMove = Input.GetAxis("Fire3");
        if (_armMove > 0 && _allowSwing)
        {
            Debug.Log("ACTION!");
            _allowSwing = false;
            transform.RotateAround(transform.position, Vector3.forward, 135);
             
            Invoke(nameof(ResetSwingCooldown),_coolTime);
        }
    }

    private void ResetSwingCooldown()
    {
        transform.RotateAround(transform.position, Vector3.back, 135);
        
        _allowSwing = true;
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
