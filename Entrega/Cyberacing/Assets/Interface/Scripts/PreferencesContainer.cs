using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreferencesContainer : MonoBehaviour
{
    public float musicLevel = 1.0f;
    public float effectsLevel = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
