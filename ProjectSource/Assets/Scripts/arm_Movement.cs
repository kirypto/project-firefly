using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm_Movement : MonoBehaviour
{
    private float armMove;
    private bool allowSwing;
    private Transform startRotation;
    private float coolTime;
    
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        armMove = 2.0f;
        allowSwing = true;

        coolTime = 2.0f;
        startRotation = transform;

    }

    // Update is called once per frame
    void Update()
    {
        
       armMove = Input.GetAxis("Fire3");
        if (armMove > 0 && allowSwing)
        {
            Debug.Log("ACTION!");
            allowSwing = false;
            transform.RotateAround(transform.position, Vector3.forward, 8000);
             
            Invoke(nameof(resetSwingCooldown),coolTime);
        }
    }

    private void resetSwingCooldown()
    {
        transform.RotateAround(transform.position, Vector3.back, 8000);
        
        allowSwing = true;
    }
}
