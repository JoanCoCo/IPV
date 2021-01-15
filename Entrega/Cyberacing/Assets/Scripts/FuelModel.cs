using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelModel : MonoBehaviour
{
    /*public float angularSpeed;
    public float low;
    public float high;
    public float verticalSpeed;
    private Vector3 verticalSpeedV;
    private bool readyToChange;
    private float timer = 0.0f;
    private float waitTime = 0.5f;*/

    // Start is called before the first frame update
    void Start()
    {
        //verticalSpeedV = new Vector3(0.0f, -1 * verticalSpeed, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        /*transform.Rotate(0.0f, angularSpeed * Time.deltaTime, 0.0f, Space.Self);
        if(readyToChange && (transform.position.y < low || transform.position.y > high)) { 
            verticalSpeedV = -1 * verticalSpeedV;
            readyToChange = false;
            timer = 0.0f;
        } else if(!readyToChange && timer > waitTime) {
            readyToChange = true;
        }
        timer += Time.deltaTime;
        transform.Translate(verticalSpeedV * Time.deltaTime);*/
    }
}
