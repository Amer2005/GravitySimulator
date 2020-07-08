using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBody : MonoBehaviour
{
    private float mass;
    private float radius;
    private float lastRadius;
    private Vector3 StartVelocity;
    Vector3 velocityNow;
    Rigidbody rb;
    BodyController bodyController;
    SphereCollider col;

    // Start is called before the first frame update

    public VirtualBody(Body body)
    {
        mass = body.Mass;
        radius = body.radius;
        StartVelocity = body.StartVelocity;
    }

}
