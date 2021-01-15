using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Saver : MonoBehaviour
{
    public string[] thingsToSave;
    private Dictionary<string, string> savedContents = new Dictionary<string, string>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        for (int i = 0; i < thingsToSave.Length; i++)
        {
            savedContents.Add(thingsToSave[i], "");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Save(string key, string value) {
        savedContents[key] = value;
    }

    public string Load(string key) {
        return savedContents[key];
    }
}
