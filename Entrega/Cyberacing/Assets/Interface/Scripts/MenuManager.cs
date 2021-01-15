using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonesMenuPrincipal { MP_Correr, MP_Opciones, MP_Salir, MP_TotalBotones };

public class MenuManager : MonoBehaviour
{
    string[] nombreBoton = {"BotonCorrer",
                            "BotonOpciones",
                            "BotonSalir"};
    public Button[] botones;
    private int botonSeleccionado = 0;
    private bool keyPressed = false;

    public GameObject preferences;

    void Start()
    {
        botones = new Button[(int)BotonesMenuPrincipal.MP_TotalBotones];

        for (int i = (int)BotonesMenuPrincipal.MP_Correr; i< (int)BotonesMenuPrincipal.MP_TotalBotones;i++)
            botones[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        botones[(int)BotonesMenuPrincipal.MP_Correr].onClick.AddListener(correrClicked);
        botones[(int)BotonesMenuPrincipal.MP_Opciones].onClick.AddListener(opcioncesClicked);
        botones[(int)BotonesMenuPrincipal.MP_Salir].onClick.AddListener(salirClicked);
        botones[0].Select();
        if(GameObject.FindWithTag("PreferencesContainer") == null) {
            GameObject o = Instantiate(preferences);
            o.GetComponent<PreferencesContainer>().musicLevel = 1.0f;
            o.GetComponent<PreferencesContainer>().effectsLevel = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            salirClicked();
        }

        if (Input.GetKey(KeyCode.Return)) {
            switch (nombreBoton[botonSeleccionado])
            {
                case "BotonOpciones":
                    opcioncesClicked();
                    break;
                case "BotonSalir":
                    salirClicked();
                    break;
                default:
                    correrClicked();
                    break;
            }
        }

        int numBotones = (int) BotonesMenuPrincipal.MP_TotalBotones;
        if (Input.GetKey(KeyCode.UpArrow) && !keyPressed) {
            botonSeleccionado = (botonSeleccionado + numBotones - 1) % numBotones;
            keyPressed = true;
            updateButtonSelected();
        } else if (Input.GetKey(KeyCode.DownArrow) && !keyPressed){
            botonSeleccionado = (botonSeleccionado + 1) % numBotones;
            keyPressed = true;
            updateButtonSelected();
        }

        if((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) && keyPressed) {
            keyPressed = false;
        }
    }

    void correrClicked()
    {
        SceneManager.LoadScene("SeleccionNivel");
    }

    void salirClicked() {
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    void opcioncesClicked() {
        SceneManager.LoadScene("MenuOpciones");
    }

    void updateButtonSelected() {
        botones[botonSeleccionado].Select();
    }
}
