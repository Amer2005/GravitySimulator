using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOrbit : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Drawer;
    public GameObject Dot;
    public GameObject Dots;

    const int numberOfTurns = 10000;

    const int howOftenToPlaceDot = 1;

    float nextActionTime = 0f;
    float period = 1f;

    bool started = false;

    void Start()
    {
        RenderPath();
    }

    public void StartButton()
    {
        started = !started;
    }

    public void RenderPath()
    {
        var GameObjectBodies = GameObject.FindGameObjectsWithTag("body");

        var bodies = new Body[GameObjectBodies.Length];

        var newPos = new Transform[GameObjectBodies.Length];

        for (int i = 0; i < GameObjectBodies.Length; i++)
        {
            bodies[i] = GameObjectBodies[i].GetComponent<Body>();

            GameObjectBodies[i].transform.localScale = new Vector3(bodies[i].radius, bodies[i].radius, bodies[i].radius);

            bodies[i].ResetBody();
        }

        var VirtualGameObjectBodies = new GameObject[GameObjectBodies.Length];

        for (int i = 0; i < VirtualGameObjectBodies.Length; i++)
        {
            VirtualGameObjectBodies[i] = Instantiate(Drawer, GameObjectBodies[i].transform.position, new Quaternion());
        }

        var virtualBodies = new Body[VirtualGameObjectBodies.Length];

        for (int i = 0; i < VirtualGameObjectBodies.Length; i++)
        {
            virtualBodies[i] = VirtualGameObjectBodies[i].GetComponent<Body>();

            virtualBodies[i].radius = bodies[i].radius;
            virtualBodies[i].Mass = bodies[i].Mass;
            virtualBodies[i].StartVelocity = bodies[i].StartVelocity;
            virtualBodies[i].ResetBody();

            VirtualGameObjectBodies[i].transform.localScale = new Vector3(bodies[i].radius, bodies[i].radius, bodies[i].radius);

            //bodies[i].ResetBody();
        }

        var Points = new Vector3[numberOfTurns, virtualBodies.Length];

        for (int i = 0; i < numberOfTurns; i++)
        {
            for (int j = 0; j < virtualBodies.Length; j++)
            {
                virtualBodies[j].UpdateVelocity(virtualBodies, Universe.physicsTimeStep);
            }

            for (int j = 0; j < virtualBodies.Length; j++)
            {
                virtualBodies[j].UpdatePositon(Universe.physicsTimeStep);

                if (i % howOftenToPlaceDot == 0)
                {
                    Points[i / howOftenToPlaceDot, j] = virtualBodies[j].rb.position;
                }
            }
        }
        for (int j = 0; j < VirtualGameObjectBodies.Length; j++)
        {
            for (int i = howOftenToPlaceDot; i < numberOfTurns; i++)
            {
                if (i % howOftenToPlaceDot == 0)
                {
                    Debug.DrawLine(Points[i / howOftenToPlaceDot - 1, j], Points[i / howOftenToPlaceDot, j], Color.white, 1, false);
                }
            }
        }

        for (int i = 0; i < VirtualGameObjectBodies.Length; i++)
        {
            Destroy(VirtualGameObjectBodies[i]);
        }

    }

    public void ClearDots()
    {
        var dots = GameObject.FindGameObjectsWithTag("Dot");

        foreach (var dot in dots)
        {
            Destroy(dot);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Time.time > nextActionTime)
        {
            nextActionTime += period;
            if (!started)
            {
                RenderPath();
            }
        }
    }
}
