using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kartController : MonoBehaviour
{

    public Rigidbody sphere;

    public float acceleration = 30f;
    public float steering = 80f;
    public float gravity = 10f;
    public float grip = 5f; //road grip, max dif between desired steering direction and current speed direction in angles

    Vector3 offset = new Vector3(0, 1.4f, 0);
    float speed, currentSpeed;
    float rotate, currentRotate;

    int dir;
    Vector3 lastPos;

    // Start is called before the first frame update
    void Start()
    {
        //inicializacion
        offset = sphere.transform.position - transform.position;
        //start the direction calc
        lastPos = sphere.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //follow collider
        transform.position = sphere.transform.position - offset;

        //input calculation
        //acceleration
        if (Input.GetButton("Fire1"))
        {
            speed = acceleration;
        }
        //steer
        if (Input.GetAxis("Horizontal") != 0)
        {
            dir = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs((Input.GetAxis("Horizontal")));
            Steer(dir, amount);
        }

        currentSpeed = Mathf.SmoothStep(currentSpeed, speed, Time.deltaTime * 12f); speed = 0f;
        currentRotate = Mathf.Lerp(currentRotate, rotate, Time.deltaTime * 4f); rotate = 0f;
    }

    private void FixedUpdate()
    {
        //physics calcs
        Debug.Log(transform.position + "    " + sphere.transform.position);
        //acceleration
        sphere.AddForce(transform.forward * currentSpeed, ForceMode.VelocityChange);
        //steering
        Vector3 desiredDir = new Vector3(0, transform.eulerAngles.y + currentRotate, 0);
        Vector3 currentDir = getCurrentSpeed();

        Debug.DrawRay(transform.position, transform.eulerAngles, Color.blue);
        Debug.DrawRay(transform.position, currentDir, Color.red);

        //if (Vector3.Angle(desiredDir, currentDir) <= grip)
        //{
            //there's still grip
        //    GetComponent<Renderer>().material.SetColor("_Color", Color.blue); //set color to blue to signal there's still grip
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, desiredDir, Time.deltaTime * 5f);
        //}
        //else
        //{
            //there's no more grip, let's clamp for now
        //    GetComponent<Renderer>().material.SetColor("_Color", Color.red); //set color to red to signal there's no more grip
        //    transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, currentDir.y, 0) + new Vector3(0, grip * dir, 0), Time.deltaTime * 5f);
        //}

        transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, desiredDir, Time.deltaTime * 5f);
        //gravity
        sphere.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    public void Steer(int direction, float amount)
    {
        rotate = (steering * direction) * amount * (1/(1 + (0.02f * currentSpeed)));
    }

    public Vector3 getCurrentSpeed()
    {
        Vector3 dir = sphere.transform.position - lastPos;
        lastPos = sphere.transform.position;
        return dir;
    }

}
