using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    public Rigidbody carRigibody;

    private AudioSource MotorSound;
    // Start is called before the first frame update
    void Start()
    {
        MotorSound = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float maxRPM = 60;
        float baseRPM = 30;

        float speed = carRigibody.velocity.magnitude;
        int gear = Mathf.FloorToInt(speed / maxRPM) + 1;
        float rpm = (speed % maxRPM) + baseRPM;
        //Debug.Log("Speed: " + speed + "Gear: " + gear + ", rpm: " + rpm);
        MotorSound.pitch = rpm / 30 ;
    }
}
