using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelUnit : MonoBehaviour
{
    public GameObject fuelModel;
    public float spawnTime;
    private GameObject instance;
    private bool isEmpty;
    private float timeStamp = 0.0f;

    public KartControllerV3 player;
    public float fuelAmount;
    public AudioSource soundEffect;

    // Start is called before the first frame update
    void Start()
    {
        createFuel();
        soundEffect = GetComponent<AudioSource>();
        float efl = GameObject.FindWithTag("PreferencesContainer").GetComponent<PreferencesContainer>().effectsLevel;
        soundEffect.GetComponent<AudioSource>().volume = efl;
    }

    // Update is called once per frame
    void Update()
    {
        if(isEmpty) {
            timeStamp += Time.deltaTime;
        }

        if(isEmpty && timeStamp >= spawnTime) {
            createFuel();
        }
    }

    void OnTriggerEnter(Collider other) {
        if(!isEmpty) { 
            destroyFuel(); 
            if(other.gameObject.tag == "Player") {
                player.fuelLevel += fuelAmount;
                if(player.fuelLevel > 100.0f) { player.fuelLevel = 100.0f; }
            }
        }
    }

    void createFuel() {
        instance = Instantiate(fuelModel, transform.position, transform.rotation);
        isEmpty = false;
    }

    void destroyFuel() {
        Destroy(instance);
        isEmpty = true;
        timeStamp = 0.0f;
        soundEffect.Play();
    }
}
