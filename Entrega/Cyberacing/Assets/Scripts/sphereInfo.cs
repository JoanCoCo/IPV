using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphereInfo : MonoBehaviour
{
    public bool grounded = true;

    public KartControllerV3 kc;

    // Start is called before the first frame update
    void Start()
    {
        //this is where I'd put my initializations... IF I HAD ANY
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.layer.Equals(2))
        {
            //only check as grounded if touching something in layer other than ignore raycast (layer of player)
            grounded = true;
        }
    }
}
