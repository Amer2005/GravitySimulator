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

    const int howOftenToPlaceDot = 50;

    void Start()
    {
        RenderPath();
    }

    public void RenderPath()
    {
        ClearDots();

        var GameObjectBodies = GameObject.FindGameObjectsWithTag("body");

        var bodies = new Body[GameObjectBodies.Length];

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
                    var dot = Instantiate(Dot, virtualBodies[j].rb.position, new Quaternion());

                    dot.transform.parent = Dots.transform;

                    var dotMesh = dot.GetComponent<MeshRenderer>();

                    dotMesh.material = GameObjectBodies[j].GetComponent<MeshRenderer>().material;
                }

                Points[i, j] = virtualBodies[j].rb.position;
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
    void Update()
    {
        
    }
}
