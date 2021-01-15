using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{
    public Transform WayPoints;
    public float acceletarion;
    public float maxSpeed;
    public float turnSpeed;
    public RaceManager RaceManager;

    private Rigidbody me;
    private int currentWayPoint;
    private float currentSpeed;
    private List<Transform> waypoints;
    // Start is called before the first frame update
    void nexWayPoint()
    {
        currentWayPoint = (currentWayPoint + 1) % waypoints.Count;
    }

    void Start()
    {
        waypoints = new List<Transform>();
        foreach(Transform tr in WayPoints)
        {
            waypoints.Add(tr);
        }
        Debug.Log(waypoints[0].name);
        me = GetComponent<Rigidbody>();
        currentSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Transform waypoint = waypoints[currentWayPoint];
        bool cercaGiro = (transform.position - waypoint.position).magnitude < 150;
        var rotation = Quaternion.LookRotation(waypoint.position - transform.position);
        //Make the rotation nice and smooth.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);

        float dynamicMaxSpeed = RaceManager.getPlayerPos() > 1? 90: 140;

        if (cercaGiro && currentSpeed > 70)
        {
            currentSpeed -= 0.1f;
        }
        else
        {
            if (currentSpeed < dynamicMaxSpeed)
                currentSpeed += acceletarion;
            else
                currentSpeed -= 0.2f;
        }
        me.velocity = transform.forward * currentSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isWayPoint = other.transform.parent.name.Equals("WayPoints");
        if (isWayPoint)
        {
            nexWayPoint();
        }
        else
        {
            Debug.Log("OtherColision");
        }
    }
}
