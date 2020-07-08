using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public float Mass;
    public float radius;
    float lastRadius;
    public Vector3 StartVelocity;
    Vector3 velocityNow;
    public Rigidbody rb;
    BodyController bodyController;
    public SphereCollider col;

    bool center;

    // Start is called before the first frame update

    private void Awake()
    {
        center = true;
        rb = GetComponent<Rigidbody>();
        var bodyControllerGameObject = GameObject.FindGameObjectWithTag("bodyController");
        bodyController = bodyControllerGameObject.GetComponent<BodyController>();
        col = gameObject.GetComponent<SphereCollider>();
        ResetBody();
        //lastRadius = radius;
    }

    public void ResetBody()
    {
        velocityNow = StartVelocity;
        col.isTrigger = true;
    }

    public void UpdateVelocity(Body[] bodies, float timeStep)
    {
        col.isTrigger = false;
        foreach (var body in bodies)
        {
            if(body != this)
            {
                float sqrtDst = (body.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (body.rb.position - rb.position).normalized;
                Vector3 force = forceDir * Universe.gravitationalConstant * Mass * body.Mass / sqrtDst;
                Vector3 acceleration = force / Mass;
                velocityNow += acceleration * timeStep;
            }
        }
        StartVelocity = velocityNow;
    }

    public void UpdatePositon(Vector3 middle, float timeStep)
    {
        rb.MovePosition(rb.position + velocityNow * timeStep);
        if(center) rb.position = rb.position - middle;
    }

    private void FixedUpdate()
    {
        if(radius != lastRadius)
        {
            gameObject.transform.localScale = new Vector3(radius, radius, radius);
            lastRadius = radius;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        bodyController.ResetSimulation();
    }
}
