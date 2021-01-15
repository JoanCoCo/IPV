using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownManager : MonoBehaviour
{
    private bool hasStarted = false;
    private float duration;

    enum Fases {GROWING, STAYING, UNGROWING, STOP};
    
    public Text label;
    public Vector3 maxScale;
    public float stayingTimePercent;
    private float fadingTimePercent;
    private Vector3 fadingSpeed;
    private Fases state;
    private int num = 0;
    private int lastNum = 3;
    private float elapsedTime2 = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        label.text = "";
        state = Fases.GROWING;
        label.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        StartCountDown(5.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(hasStarted) {
            elapsedTime2 += Time.unscaledDeltaTime;
            if(num <= lastNum + 1) {
                switch (state)
                {
                    case Fases.GROWING:
                        label.transform.localScale += fadingSpeed * Time.unscaledDeltaTime;
                        if(label.transform.localScale.magnitude >= maxScale.magnitude) {
                            state = Fases.STAYING;
                        }
                        break;
                    case Fases.UNGROWING:
                        label.transform.localScale -= fadingSpeed * Time.unscaledDeltaTime;
                        if(elapsedTime2 >= (duration /5.0f) * stayingTimePercent + 2.0f * (duration /5.0f) * fadingTimePercent) {
                            label.transform.localScale = Vector3.zero;
                            if(num == lastNum) {
                                label.text = "Go!";
                                num += 1;
                                state = Fases.GROWING;
                            }else if(num > lastNum) {
                                state = Fases.STOP;
                                Destroy(gameObject);
                            } else {
                                num += 1;
                                label.text = num.ToString();
                                state = Fases.GROWING;
                            }
                            elapsedTime2 = 0.0f;
                        }
                        break;
                    case Fases.STAYING:
                        if(elapsedTime2 >= (duration /5.0f) * stayingTimePercent + (duration /5.0f) * fadingTimePercent) {
                            state = Fases.UNGROWING;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void StartCountDown(float d) {
        duration = d;
        fadingTimePercent = (1.0f - stayingTimePercent) / 2.0f;
        fadingSpeed = maxScale / ((d / 5.0f) * fadingTimePercent);
        hasStarted = true;
    }
}
