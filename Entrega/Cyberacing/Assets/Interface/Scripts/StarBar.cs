using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBar : MonoBehaviour
{
    public Button[] stars;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < stars.Length; i++) {
            stars[i].interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int n) {
        for(int i = 0; i < n && i < stars.Length; i++) {
            stars[i].interactable = true;
        }
    }
}
