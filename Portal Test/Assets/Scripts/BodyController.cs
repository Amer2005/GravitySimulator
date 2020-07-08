using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BodyController : MonoBehaviour
{
    // Start is called before the first frame update

    Body[] bodies;

    public bool started;

    public GameObject Middle;

    public Text ButtonText;

    void Start()
    {
        started = false;
    }
    // Update is called once per frame

    void bodyStart()
    {
        ButtonText.text = "Stop";
        started = true;
        var GameObjectBodies = GameObject.FindGameObjectsWithTag("body");

        bodies = new Body[GameObjectBodies.Length];

        for (int i = 0; i < GameObjectBodies.Length; i++)
        {
            bodies[i] = GameObjectBodies[i].GetComponent<Body>();

            GameObjectBodies[i].transform.localScale = new Vector3(bodies[i].radius, bodies[i].radius, bodies[i].radius);

            bodies[i].ResetBody();
        }

        foreach (var body in GameObjectBodies)
        {
            body.GetComponent<Body>();
        }

        Time.fixedDeltaTime = Universe.physicsTimeStep;
    }

    void FixedUpdate()
    {
        if (started)
        {
            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].UpdateVelocity(bodies, Universe.physicsTimeStep);
            }

            for (int i = 0; i < bodies.Length; i++)
            {
                bodies[i].UpdatePositon(Middle.transform.position, Universe.physicsTimeStep);
            }
        }
    }

    public void StartButtonClicked()
    {
        if (!started)
        {
            bodyStart();
        }
        else
        {
            ResetSimulation();
        }
    }

    public void ResetSimulation()
    {
        started = false;

        ButtonText.text = "Start";

        foreach (var body in bodies)
        {
            body.ResetBody();
        }
    }
}
