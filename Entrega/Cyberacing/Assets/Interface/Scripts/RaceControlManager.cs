using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RaceControlManager : MonoBehaviour
{
    public Text speedDisplay;
    public Image fuelDisplay;
    public float fuelStartAngle;
    public float fuelEndAngle;
    public Text cronoDisplay;
    public Text positionDisplay;
    public Text lapDisplay;
    public int totalNumberOfLaps;
    public PauseMenuManager pauseScreen;
    public GameObject alertTint;
    public int dangerFuelLevel;

    public bool isPaused = false;
    private bool locked = false;
    private float elapsedTime = 0.0f;

    public CountDownManager countDownScreen;
    public FinishedManager finishedScreen;

    public GameObject audioSourceObject;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.FindWithTag("PreferencesContainer") != null) {
            float ml = GameObject.FindWithTag("PreferencesContainer").GetComponent<PreferencesContainer>().musicLevel;
            audioSourceObject.GetComponent<AudioSource>().volume = ml;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Tab) && !locked) {
            if(!isPaused) {
                showPauseScreen();
                isPaused = true;
            }
        }

        if(isPaused) {
            locked = true;
        }

        if(locked && !isPaused) {
            elapsedTime += Time.deltaTime;
            if(elapsedTime > 0.2f) {
                locked = false;
                elapsedTime = 0.0f;
            }
        }
    }
    
    // Updates the chrono label given an amount of time in seconds.
    public void UpdateChrono(float time) {
        float min = Mathf.Floor(time / 60.0f);
        float secs = time - min * 60.0f;
        float mlsecs = (secs - Mathf.Floor(secs)) * 1000.0f;
        UpdateChrono((int)mlsecs, (int)secs, (int)min);
    }

    // Updates the chrono label given an amount of time in milliseconds, seconds and minutes.
    public void UpdateChrono(int milliSeconds, int seconds, int minutes) {
        string secs = seconds.ToString();
        string min = minutes.ToString();
        string mlsecs = milliSeconds.ToString();

        if(seconds < 10) {
            secs = "0" + secs;
        }

        if(minutes < 10) {
            min = "0" + min;
        }

        if(milliSeconds < 10) {
            mlsecs = "0" + mlsecs;
        }

        if(milliSeconds < 100) {
            mlsecs = "0" + mlsecs;
        }

        cronoDisplay.text = min + ":" + secs + ":" + mlsecs;
    }

    // Updates the speed label given an amount of speed in km/h.
    public void UpdateSpeed(int speed) {
        speedDisplay.text = speed.ToString();
    }

    // Updates the position label given the position in the race.
    public void UpdatePosition(int position) {
        positionDisplay.text = position.ToString() + "a Posición";
    }

    // Updates the fuelmeter given an amount of fuel in %.
    public void UpdateFuelLevel(int level) {
        float angle = fuelStartAngle + ((100 - level) * (fuelEndAngle - fuelStartAngle) / 100);
        fuelDisplay.transform.eulerAngles = new Vector3(0, 0, angle);
        if(level <= dangerFuelLevel) {
            Image image = alertTint.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f, (float)(dangerFuelLevel - level)/(float)dangerFuelLevel);
        } else {
            Image image = alertTint.GetComponent<Image>();
            image.color = new Color(1f, 1f, 1f, 0f);
        }
    }

    // Updates the lap label given the current number of lap.
    public void UpdateLap(int lap) {
        lapDisplay.text = "Vuela " + lap.ToString() + "/" + totalNumberOfLaps.ToString();
    }

    // Shows the pause menu.
    public void showPauseScreen() {
        PauseMenuManager p = Instantiate(pauseScreen, transform.position, transform.rotation);
        Time.timeScale = 0;
        p.mainWindow.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(p.mainWindow);
        AudioListener.pause = true;
    }

    public void unPause() {
        isPaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
    }

    public void ShowCountDown(float d) {
        CountDownManager aux = Instantiate(countDownScreen);
        aux.StartCountDown(d);
    }

    public void ShowFinished(float d) {
        FinishedManager aux = Instantiate(finishedScreen);
        aux.StartFinish(d);
    }
}
