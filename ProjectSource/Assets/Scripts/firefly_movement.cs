using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firefly_movement : MonoBehaviour
{

    private Vector2 startPosition;
    private Vector2 direction;
    public Collider2D BoundingBox;
    public float FlySpeed = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        BoundingBox = gameObject.GetComponentInParent(typeof(BoxCollider2D)) as Collider2D;
        startPosition = transform.position;
        direction = Random.insideUnitSphere;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3) direction * FlySpeed;
        if(!BoundingBox.bounds.Contains(transform.position)) {
            direction = Random.insideUnitCircle;
            direction += (startPosition - (Vector2) transform.position).normalized * 0.25f;
        }
    }
}
