using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kartControllerv2 : MonoBehaviour
{

    public float acceleration = 1f;
    public float grip = 5f;

    public float gravity = 10f;
    public Rigidbody rb;

    float aIn;  //acceleration input
    float sIn;  //steering input

    float clampHeight;
    Vector3 desiredPos;

    // Start is called before the first frame update
    void Start()
    {
        clampHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //get inputs
        aIn = Input.GetAxis("Fire1");
        sIn = Input.GetAxis("Horizontal");

        //Vector3 desiredPos = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + (sIn * grip), transform.eulerAngles.z) * aIn * acceleration;
        desiredPos = Quaternion.Euler(0, sIn * grip, 0) * transform.forward * aIn * acceleration;
    }

    private void FixedUpdate()
    {
        //calculate physics
        Debug.DrawRay(transform.position, desiredPos*3, Color.red);
        Debug.DrawRay(transform.position, transform.forward*3, Color.blue);
        moveTowards(desiredPos);
    }

    private void moveTowards(Vector3 direction)
    {
        //move towards direction
        transform.position = lerpVector(transform.position, transform.position + direction, Time.deltaTime);
        //rb.AddForce(direction, ForceMode.VelocityChange);

        //look towards movement
        transform.forward = lerpVector(transform.forward, direction, Time.deltaTime);
    }

    private Vector3 lerpVector(Vector3 v1, Vector3 v2, float t)
    {
        return new Vector3(
            Mathf.Lerp(v1.x, v2.x, t),
            Mathf.Lerp(v1.y, v2.y, t),
            Mathf.Lerp(v1.z, v2.z, t)
                );
    }
}
