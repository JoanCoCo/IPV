using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartControllerV3 : MonoBehaviour
{
    //public references
    public Rigidbody sphere;
    public Text debugText;
    public float gravity = 10f;
    public float sizeMultiplier;

    //car stats
    public float acceleration = 30f;
    public float maxSpeed = 50f;
    public float steer = 80f;
    public float minDriftingSpeed = 3f;
    public float minDriftDir, maxDriftDir;

    public RaceControlManager controls;
    public float fuelLevel = 100.0f;
    public float fuelConsumedByKm = 10.0f;

    //internal variables
    Vector3 offset = new Vector3(0, 1.4f, 0);
    Vector2 movementInput, objectiveDir, currentDir; //[acceleration, steering]
    int dir = 1, driftDir = 1;
    bool isDrifting;
    float strAmount;

    // Start is called before the first frame update
    void Start()
    {
        offset = sphere.transform.position - transform.position;
        sphere.transform.forward = transform.forward;
        isDrifting = false;
    }

    // Update is called once per frame
    void Update()
    {
        //follow collider
        transform.position = sphere.transform.position - offset;

        //input calculation
        movementInput[0] = Input.GetAxis("Accelerate_gc") != 0 ? Input.GetAxis("Accelerate_gc") : Input.GetAxis("Accelerate_kb");
        movementInput[1] = Input.GetAxis("Horizontal");

        //what happens when drifting begins
        if(Input.GetButtonDown("Drift") && !isDrifting && isDriftable() && !paddedEquals(movementInput[1], 0, 0.4f))
        {
            isDrifting = true;
            driftDir = movementInput[1] > 0 ? 1 : -1;
        }
        //what happens when drifting ends
        if((Input.GetButtonUp("Drift") || !isDriftable()) && isDrifting)
        {
            isDrifting = false;
        }

        //acceleration
        if (!paddedEquals(movementInput[0], 0, 0.01f))
        {
            objectiveDir[0] = movementInput[0] * acceleration;
        }
        //steer
        if (isDrifting)
        {
            float amount = driftDir == 1 ? remap(movementInput[1], -1, 1, minDriftDir, maxDriftDir) : remap(movementInput[1], -1, 1, -maxDriftDir, -minDriftDir);
            strAmount = amount;
            objectiveDir[1] = (steer) * amount * (1 / (1 + (0.02f * currentDir[0])));
        }
        else
        {
            float direction = movementInput[1] > 0 ? 1 : -1;
            float amount = Mathf.Abs((movementInput[1]));
            strAmount = direction * amount;
            objectiveDir[1] = (steer * direction) * amount * (1 / (1 + (0.02f * currentDir[0])));
        }
        currentDir[0] = Mathf.SmoothStep(currentDir[0], objectiveDir[0], Time.deltaTime * 12f); objectiveDir[0] = 0f;
        currentDir[1] = isGrounded()? Mathf.Lerp(currentDir[1], objectiveDir[1], Time.deltaTime * 4f) : currentDir[1]; objectiveDir[1] = 0f;

        setDebugText();
    }

    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, transform.up, currentDir[1]);
        Debug.DrawRay(transform.position, transform.forward * 3 * sizeMultiplier, Color.red);
        //this if not drifting
        //change direction
        sphere.transform.RotateAround(sphere.transform.position, transform.up, currentDir[1]);
        Debug.DrawRay(transform.position, sphere.transform.forward * 2 * sizeMultiplier, Color.green);
        //change speed
        if (isGrounded())
        {
            Debug.DrawRay(transform.position, -transform.up * 1.1f * sizeMultiplier, Color.green);
            sphere.AddForce(transform.forward * currentDir[0], ForceMode.Acceleration);
            if(sphere.velocity.magnitude < 0.3f)
            {
                dir = currentDir[0] >= 0 ? 1 : -1;
            }
            if (!isDrifting) 
                sphere.velocity = Vector3.Lerp(sphere.velocity, Vector3.Project(sphere.velocity, dir * transform.forward), Time.deltaTime * 10.0f);


        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up * 1.1f * sizeMultiplier, Color.green);
            sphere.AddForce(Vector3.down * gravity, ForceMode.Force);
        }
        

        //rotation based on terrain
        RaycastHit hit;
        //detect ground with raycast
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f * sizeMultiplier))
        {
            Debug.DrawRay(transform.position, Vector3.down * 1.2f * sizeMultiplier, Color.green);
            float diffTerrain = Vector3.Angle(transform.up, hit.normal);
            Debug.DrawRay(transform.position, transform.up * 3 * sizeMultiplier, Color.white);
            Debug.DrawRay(transform.position, hit.normal * 3 * sizeMultiplier, Color.yellow);
            if(!paddedEquals(diffTerrain, 0, 0.01f))
            {
                Vector3 newForward = Quaternion.FromToRotation(transform.up, hit.normal) * transform.forward;
                Debug.DrawRay(transform.position, transform.forward * 3 * sizeMultiplier, Color.white);
                Debug.DrawRay(transform.position, newForward * 3 * sizeMultiplier, Color.yellow);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newForward, hit.normal), Time.deltaTime * 8f);
            }
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.down * 1.2f * sizeMultiplier, Color.red);
        }
        float sSpeed = sphere.velocity.magnitude;
        controls.UpdateSpeed((int)sSpeed);
        fuelLevel -= fuelConsumedByKm * sSpeed * (Time.deltaTime / 3600.0f);
        controls.UpdateFuelLevel((int)fuelLevel);
        if(fuelLevel <= 0.0f) {
            foreach (GameObject e in GameObject.FindGameObjectsWithTag("Player"))
            {
                e.tag = "NoPlayer";
            }
        }
    }

    /// <summary>
    /// Checks whether x and y are equal with a delta range of maximum difference
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="delta"></param>
    /// <returns></returns>
    private bool paddedEquals(float x, float y, float delta)
    {
        return x >= y - delta && x <= y + delta;
    }

    /// <summary>
    /// maps the given value from the range from1-to1 to a new range from2-to2
    /// </summary>
    /// <param name="value"></param>
    /// <param name="from1"></param>
    /// <param name="to1"></param>
    /// <param name="from2"></param>
    /// <param name="to2"></param>
    /// <returns></returns>
    private float remap(float value, float from1, float to1, float from2, float to2)
    {
        return ((value - from1) / (to1 - from1)) * (to2 - from2) + from2;
    }

    /// <summary>
    /// Sets the debug text on screen
    /// </summary>
    private void setDebugText()
    {
        string ac = "acceleration:    " + movementInput[0] * acceleration;
        string speed = "speed:  " + sphere.velocity.magnitude + "\ndirection:   " + transform.forward;
        string rotation = "rotation:    " + transform.eulerAngles + "\nlocal rotation:  " + transform.localEulerAngles;
        string grounded = "grounded:  " + isGrounded();
        string drift = "driftable:  " + isDriftable() + "\ndrifting:    " + isDrifting;

        //debugText.text = ac + "\n" + speed + "\n" + rotation + "\n" + grounded + "\n" + drift + "\n" + strAmount;
    }

    /// <summary>
    /// returns whether the sphere collider is touching the ground or not
    /// </summary>
    /// <returns></returns>
    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, 1.1f * sizeMultiplier);
    }

    private bool isDriftable()
    {
        return /*isGrounded() && */ sphere.velocity.magnitude >= minDriftingSpeed;
    }
}
