using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishedManager : MonoBehaviour
{
    private bool hasStarted = false;
    private float duration;
    private float elapsedTime = 0.0f;

    enum Fases {GROWING, STAYING, UNGROWING, STOP};
    
    public Text label;
    public Vector3 maxScale;
    public float stayingTimePercent;
    private float fadingTimePercent;
    private Vector3 fadingSpeed;
    private Fases state;

    // Start is called before the first frame update
    void Start()
    {
        state = Fases.GROWING;
        label.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted) {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= duration) {
                Destroy(gameObject);
            } else {
                switch (state)
                {
                    case Fases.GROWING:
                        label.transform.localScale += fadingSpeed * Time.deltaTime;
                        if(label.transform.localScale.magnitude >= maxScale.magnitude) {
                            state = Fases.STAYING;
                        }
                        break;
                    case Fases.UNGROWING:
                        label.transform.localScale -= fadingSpeed * Time.deltaTime;
                        if(elapsedTime >= duration * stayingTimePercent + 2.0f * duration * fadingTimePercent) {
                            label.transform.localScale = Vector3.zero;
                            state = Fases.STOP;
                        }
                        break;
                    case Fases.STAYING:
                        if(elapsedTime >= duration * stayingTimePercent + duration * fadingTimePercent) {
                            state = Fases.UNGROWING;
                        }
                        break;
                    default:
                        break;
                }
                
            }
        }
    }

    public void StartFinish(float d) {
        hasStarted = true;
        duration = d;
        fadingTimePercent = (1.0f - stayingTimePercent) / 2.0f;
        fadingSpeed = maxScale / (d * fadingTimePercent);
    }
}
