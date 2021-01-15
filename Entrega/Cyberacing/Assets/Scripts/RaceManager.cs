using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    public RaceControlManager controls;
    public GameObject[] cars;
    public int numberOfLaps;
    private float chrono = 0.0f;
    private enum states {STARTING, STARTED, FINISHED};
    private int state;
    public float countDownDuration;
    public float closingDuration;
    private float elapsedTime;
    private int playerLap = 0;
    private int playerPos = 1;
    public Saver saver;
    private bool playerDied = false;

    private float referenceDistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        state = (int)states.STARTING;
        controls.ShowCountDown(countDownDuration);
        controls.UpdatePosition(playerPos);
        elapsedTime = 0.0f;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == (int)states.STARTED) {
            chrono += Time.deltaTime;
            controls.UpdateChrono(chrono);

            for (int i = 0; i < cars.Length - 1; i++)
            {
                Vector3 dirA = cars[i].transform.forward;
                Vector3 dirB = cars[i+1].transform.forward;
                Vector3 distAB = cars[i+1].transform.position - cars[i].transform.position;
                Vector3 distBA = cars[i].transform.position - cars[i+1].transform.position;

                if(distBA.magnitude > 100) {
                    if(cars[i].GetComponent<DistanceTracker>().distance < 
                        cars[i+1].GetComponent<DistanceTracker>().distance) {
                        GameObject tmp = cars[i];
                        cars[i] = cars[i+1];
                        cars[i+1] = tmp;

                        if(cars[i].tag == "Player") {
                            playerPos = i+1;
                        } else if(cars[i+1].tag == "Player") {
                            playerPos = i+2;
                        }
                    }
                } else {
                    float cosA = dirA.x * distAB.x + dirA.y * distAB.y + dirA.z * distAB.z;
                    float cosB = dirB.x * distBA.x + dirB.y * distBA.y + dirB.z * distBA.z;

                    //print("cosA: " + cosA.ToString());
                    //print("cosB: " + cosB.ToString());

                    if(cosA > 0 && cosB < 0) {
                        GameObject tmp = cars[i];
                        cars[i] = cars[i+1];
                        cars[i+1] = tmp;

                        if(cars[i].tag == "Player") {
                            playerPos = i+1;
                        } else if(cars[i+1].tag == "Player") {
                            playerPos = i+2;
                        }
                    }
                }

                //print("dirA: " + dirA.ToString());
                //print("dirB: " + dirB.ToString());
                //print("distAB: " + distAB.ToString());
                //print("distBA: " + distBA.ToString());

                //PrintLeaderboard();
            }
            controls.UpdatePosition(playerPos);
            if(GameObject.FindWithTag("Player") == null) {
                state = (int)states.FINISHED;
                controls.ShowFinished(closingDuration);
                playerDied = true;
            }
        } else if(state == (int)states.STARTING) {
            elapsedTime += Time.unscaledDeltaTime;
            if(GameObject.FindWithTag("CountDown") == null) {
                state = (int)states.STARTED;
                elapsedTime = 0.0f;
                Time.timeScale = 1;
            }
        } else if(state == (int)states.FINISHED) {
            elapsedTime += Time.unscaledDeltaTime;
            if(elapsedTime >= closingDuration) {
                //print("Pos: " + playerPos.ToString());
                saver.Save("Position", playerPos.ToString());
                saver.Save("Chrono", chrono.ToString());
                if(playerDied) saver.Save("Penalized", "yes"); else saver.Save("Penalized", "no");
                saver.Save("LastPosition", cars.Length.ToString());
                SceneManager.LoadScene("PantallaResultado");
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            if((playerLap == numberOfLaps && state == (int)states.STARTED 
                && other.gameObject.GetComponent<DistanceTracker>().distance >= 22000.0f) || playerDied) {
                state = (int)states.FINISHED;
                controls.ShowFinished(closingDuration);
            } else {
                if(other.gameObject.GetComponent<DistanceTracker>().distance >= referenceDistance) {
                    playerLap += 1;
                    controls.UpdateLap(playerLap);
                    referenceDistance += 5000;
                }
            }
        }
    }

    private void PrintLeaderboard() {
        for (int i = 0; i < cars.Length; i++)
        {
            //print((i+1).ToString() + ". " + cars[i].tag);
        }
    }

    public float getPlayerPos()
    {
        return this.playerPos;
    }
}
