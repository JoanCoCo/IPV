using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public StarBar stars;
    public Text position;
    public Text chrono;
    public Text reward;
    public Button retry;
    public Button next;

    private int botonSeleccionado = 1;
    private bool keyPressed = false;
    private int evaluatedLevel = 1;

    private enum botones {BotonCorrer, BotonSalir}
    private Saver saver;

    // Start is called before the first frame update
    void Start()
    {
        retry.onClick.AddListener(correrClicked);
        next.onClick.AddListener(salirClicked);
        UpdateSelectedButton();
        saver = GameObject.FindWithTag("Saver").GetComponent<Saver>();
        string failed = saver.Load("Penalized");
        if(failed == "yes") {
            int pos = int.Parse(saver.Load("LastPosition"));
            float chr = 0.0f;
            int coins = 0;
            SetPosition(pos);
            SetChrono(chr);
            SetReward(coins);
        } else {
            int pos = int.Parse(saver.Load("Position"));
            float chr = float.Parse(saver.Load("Chrono"));
            SetPosition(pos);
            SetChrono(chr);
            float coins = (((float)(16 - pos)) / (chr / (6.5f*60.0f))) * 10.0f; 
            int starsVal = (int) (Mathf.Min((6.5f*60.0f / chr), 1.0f) * 5.0f);
            SetReward((int)coins);
            SetStars(starsVal);
        }
        Destroy(GameObject.FindWithTag("Saver"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            salirClicked();
        }
        if(Input.GetKey(KeyCode.Return)) {
            switch (botonSeleccionado)
            {
                case (int)botones.BotonCorrer:
                    correrClicked();
                    break;
                case (int)botones.BotonSalir:
                    salirClicked();
                    break;
                default:
                    break;
            }
            
        }

        if((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) && !keyPressed) {
            keyPressed = true;
            if(botonSeleccionado == (int)botones.BotonCorrer) {
                botonSeleccionado = (int)botones.BotonSalir;
            } else {
                botonSeleccionado = (int)botones.BotonCorrer;
            }
            UpdateSelectedButton();
        }

        if(Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)) {
            keyPressed = false;
        }
    }

    private void UpdateSelectedButton() {
        switch (botonSeleccionado)
        {
            case (int)botones.BotonCorrer:
                retry.Select();
                break;
            case (int)botones.BotonSalir:
                next.Select();
                break;
            default:
                break;
        }
        
    }

    public void SetEvaluatedLevel(int level) {
        evaluatedLevel = level;
    }

    public void SetStars(int n) {
        stars.SetValue(n);
    }

    public void SetReward(int coins) {
        this.reward.text = "Recompensa: +" + coins.ToString() + " monedas";
    }

    // Sets the position label given the position in the race.
    public void SetPosition(int position) {
        this.position.text = position.ToString() + "a Posición";
    }

    // Sets the chrono label given an amount of time in seconds.
    public void SetChrono(float time) {
        float min = Mathf.Floor(time / 60.0f);
        float secs = time - min * 60.0f;
        float mlsecs = (secs - Mathf.Floor(secs)) * 1000.0f;
        SetChrono((int)mlsecs, (int)secs, (int)min);
    }

    // Sets the chrono label given an amount of time in milliseconds, seconds and minutes.
    public void SetChrono(int milliSeconds, int seconds, int minutes) {
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

        this.chrono.text = min + ":" + secs + ":" + mlsecs;
    }

    private void correrClicked()
    {
        SceneManager.LoadScene("Level" + evaluatedLevel.ToString());
    }

    private void salirClicked() {
        SceneManager.LoadScene("SeleccionNivel");
    }
}
