using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arm_Movement : MonoBehaviour
{
    private float armMove;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        armMove = 2.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        
       armMove = Input.GetAxis("Fire3");
        if (armMove > 0)
        {
            Debug.Log("ACTION!");
            
            transform.RotateAround(transform.position, Vector3.back, 200 * Time.deltaTime);
        }
    }
}
