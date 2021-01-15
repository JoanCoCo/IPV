using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonesPauseMenu { MP_Correr, MP_Salir, MP_TotalBotones };

public class PauseMenuManager : MonoBehaviour
{
    string[] nombreBoton = {"BotonContinuar",
                            "BotonSalir"};

    //Make sure to attach these Buttons in the Inspector
    public Button[] botones;
    private int botonSeleccionado = 0;
    private bool keyPressed = false;
    private GameObject me;

    private float elapsedTime = 0.0f;
    private bool locked = true;

    public GameObject mainWindow;

    // Start is called before the first frame update
    void Start()
    {
        me = gameObject;
        //Create the new space for the buttons
        botones = new Button[(int)BotonesPauseMenu.MP_TotalBotones];
        //Select the empty GameObject (Menu Manager) in the Hierarchy 
        //Drag and drop each one of the UI Buttons from the Hierarchy to the every Button array fields in the Inspector when the Menu Manager empty gameObject is selected

        for (int i = (int)BotonesPauseMenu.MP_Correr; i< (int)BotonesPauseMenu.MP_TotalBotones;i++)
            botones[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        //Calls the correrClicked method when you click the Button Jugar
        botones[(int)BotonesPauseMenu.MP_Correr].onClick.AddListener(continuarClicked);
        botones[(int)BotonesPauseMenu.MP_Salir].onClick.AddListener(salirClicked);
        botones[0].Select();
    }

    // Update is called once per frame
    void Update()
    {
        //Regla del escape
        if (Input.GetKey(KeyCode.Escape)) {
            salirClicked();
        }

        if(Input.GetKey(KeyCode.Tab) && !locked) {
            continuarClicked();
        }

        //Regla del enter
        if (Input.GetKey(KeyCode.Return)) {
            switch (nombreBoton[botonSeleccionado])
            {
                case "BotonSalir":
                    salirClicked();
                    break;
                default:
                    continuarClicked();
                    break;
            }
        }

        int numBotones = (int) BotonesPauseMenu.MP_TotalBotones;
        if (Input.GetKey(KeyCode.UpArrow) && !keyPressed) {
            //boton[botonSeleccionado].Deselect();
            botonSeleccionado = (botonSeleccionado + numBotones - 1) % numBotones;
            keyPressed = true;
            updateButtonSelected();
        } else if (Input.GetKey(KeyCode.DownArrow) && !keyPressed){
            //boton[botonSeleccionado].Select();
            botonSeleccionado = (botonSeleccionado + 1) % numBotones;
            keyPressed = true;
            updateButtonSelected();
        }

        if(Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
            keyPressed = false;
        }

        if(locked) {
            elapsedTime += Time.unscaledDeltaTime;
            if(elapsedTime > 0.2f) {
                locked = false;
            }
        }
    }

    void salirClicked() {
        Time.timeScale = 1;
        AudioListener.pause = false;
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void unPause() {
        RaceControlManager control = GameObject.FindWithTag("RaceControl").GetComponent<RaceControlManager>();
        control.unPause();
    }

    void continuarClicked() {
        unPause();
        closeThis();
    }

    void updateButtonSelected() {
        botones[botonSeleccionado].Select();
    }

    void closeThis() {
        Destroy(me);
    }
}
