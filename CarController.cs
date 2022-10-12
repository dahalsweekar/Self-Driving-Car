using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NNet))]
public class CarController : MonoBehaviour
{

    private Vector3 startPosition, startRotation;
    private NNet network;


    [Range(-1f, 1f)]
    public float a, t;

    public float timeSinceStart = 0f;

    [Header("Fitness")]
    public float overallFitness;
    public float distanceMultiplier = 1.4f; //Distance Travelled by the car 
    public float avgSpeedMultiplier = 0.2f; //Avg speed of the cars impacts which one is the best fit
    public float sensorMultiplier = 0.1f;

    [Header("Network Options")]
    public int LAYERS = 1;
    public int NEURONS = 10;

    private Vector3 lastPosition;
    private float totalDistanceTravelled;
    private float avgSpeed;

    private float aSensor, bSensor, cSensor; //Inputs to neural network
   
    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.eulerAngles;
        network = GetComponent<NNet>();

        //Test Code
        network.Initialise(LAYERS, NEURONS);
    }

   
    public void Reset()
    {

        //Test Code
        network.Initialise(LAYERS, NEURONS);

        timeSinceStart = 0f;
        totalDistanceTravelled = 0f;
        avgSpeed = 0f;
        lastPosition = startPosition;
        overallFitness = 0;
        transform.position = startPosition;
        transform.eulerAngles = startRotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Reset();
    }

    private void FixedUpdate()
    {
        InputSensors();
        lastPosition = transform.position;

        (a, t) = network.RunNetwork(aSensor, bSensor, cSensor);

        MoveCar(a, t);

        timeSinceStart += Time.deltaTime;

        CalculateFitness();

        //a = 0;
        //t = 0;

    }

    private void CalculateFitness()
    {
        totalDistanceTravelled += Vector3.Distance(transform.position, lastPosition);
        avgSpeed = totalDistanceTravelled / timeSinceStart;

        overallFitness = (totalDistanceTravelled * distanceMultiplier) + (avgSpeed * avgSpeedMultiplier) + (((aSensor+bSensor+cSensor)/3)*sensorMultiplier);

        if (timeSinceStart > 20 && overallFitness < 40)//useless training
        {
            Reset();
        }

        if (overallFitness >= 1000)//sucessful training
        {
            //Saves netwok to a JSON
            Reset();
        }
    }
    private void InputSensors()
    {
        Vector3 a = (transform.forward + transform.right);
        Vector3 b = transform.forward;
        Vector3 c = (transform.forward - transform.right);

        Ray r = new Ray(transform.position, a);
        RaycastHit hit; //datatype to store the value returned by RAY

        if (Physics.Raycast(r, out hit))
        {
            aSensor = hit.distance / 20;//normalizing the value between 0 and 1
            Debug.DrawLine(r.origin, hit.point, Color.red);
                   
        }

        r.direction = b;
        if(Physics.Raycast(r, out hit))
        {
            bSensor = hit.distance / 20;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

        r.direction = c;
        if(Physics.Raycast(r, out hit))
        {
            cSensor = hit.distance / 20;
            Debug.DrawLine(r.origin, hit.point, Color.red);
        }

    }

    private Vector3 inp;
        public void MoveCar(float v, float h)
    {
        inp = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, v * 11.4f), 0.02f);//Gradient acceleration
        inp = transform.TransformDirection(inp); //TransformDirection moves the car not relative to the world but car itself
        transform.position += inp;
        transform.eulerAngles += new Vector3(0, (h * 90)*0.02f, 0);//realistic smooth steering rotation
   
    }
}
