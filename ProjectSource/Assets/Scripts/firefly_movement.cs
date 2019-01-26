using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firefly_movement : MonoBehaviour
{

    private Vector2 startPosition;
    private Vector2 direction;
    Collider boundingBox;
    // Start is called before the first frame update
    void Start()
    {
        boundingBox = (Collider) GetComponentInParent(typeof(Collider));
        startPosition = transform.position;
        direction = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        if(!boundingBox.bounds.Contains(transform.position)) {
            print("Hi I'm not in the box!");
        }
    }
}
