using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public float distance;
    private Vector3 previousPos;
    // Start is called before the first frame update
    void Start()
    {
        distance = 0;
        previousPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distance += (previousPos - transform.position).magnitude;
        previousPos = transform.position;
    }
}
