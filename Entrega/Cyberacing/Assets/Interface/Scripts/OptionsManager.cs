using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

enum BotonesMenuOpciones { MO_Salir, MO_TotalBotones };

public class OptionsManager : MonoBehaviour
{
    string[] nombreBoton = {"BotonAtras"};
    public Button[] botones;
    private bool keyPressed = false;

    public Slider musicSlider;
    public Slider effectsSlider;

    public Text musicText;
    public Text effectsText;

    public Color selectedColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private int selectedItem = 0;

    public float stepSs = 0.01f;

    void Start()
    {
        botones = new Button[(int)BotonesMenuOpciones.MO_TotalBotones];

        for (int i = 0; i< (int)BotonesMenuOpciones.MO_TotalBotones;i++)
            botones[i] = GameObject.Find(nombreBoton[i]).GetComponent<Button>();

        botones[(int)BotonesMenuOpciones.MO_Salir].onClick.AddListener(salirClicked);

        musicSlider.onValueChanged.AddListener(musicValueSet);
        effectsSlider.onValueChanged.AddListener(effectsValueSet);

        if(GameObject.FindWithTag("PreferencesContainer") != null) {
            musicSlider.value = GameObject.FindWithTag("PreferencesContainer").GetComponent<PreferencesContainer>().musicLevel;
        } else {
            musicSlider.value = 1.0f;
        }

        if(GameObject.FindWithTag("PreferencesContainer") != null) {
            effectsSlider.value = GameObject.FindWithTag("PreferencesContainer").GetComponent<PreferencesContainer>().effectsLevel;
        } else {
            effectsSlider.value = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) || (Input.GetKey(KeyCode.Return) && selectedItem == 0)) {
            salirClicked();
        }

        if(Input.GetKey(KeyCode.UpArrow) && !keyPressed) {
            keyPressed = true;
            selectedItem -= 1;
            if(selectedItem < 0) selectedItem = 2;
            UpdateSelectedButton();
        }

        if(Input.GetKey(KeyCode.DownArrow) && !keyPressed) {
            keyPressed = true;
            selectedItem = (selectedItem + 1) % 3;
            UpdateSelectedButton();
        }

        if(Input.GetKey(KeyCode.LeftArrow) && !keyPressed) {
            keyPressed = true;

            switch(selectedItem) {
                case 1:
                    float vm = musicSlider.value - stepSs;
                    print(musicSlider.value);
                    if (vm < 0.0f) vm = 0.0f;
                    print(vm);  
                    musicSlider.value = vm;
                    musicValueSet(vm);
                    break;
                case 2:
                    float vf = effectsSlider.value - stepSs;
                    if (vf < 0.0f) vf = 0.0f;  
                    effectsSlider.value = vf;
                    effectsValueSet(vf);
                    break;
            }
        }

        if(Input.GetKey(KeyCode.RightArrow) && !keyPressed) {
            keyPressed = true;

            switch(selectedItem) {
                case 1:
                    float vm = musicSlider.value + stepSs;
                    print(musicSlider.value);
                    if (vm > 1.0f) vm = 1.0f;  
                    print(vm);
                    musicSlider.value = vm;
                    musicValueSet(vm);
                    break;
                case 2:
                    float vf = effectsSlider.value + stepSs;
                    if (vf > 1.0f) vf = 1.0f;  
                    effectsSlider.value = vf;
                    effectsValueSet(vf);
                    break;
            }
        }

        if((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || 
                Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && keyPressed) {
            keyPressed = false;
        }
    }

    void salirClicked() {
        SceneManager.LoadScene("MenuPrincipal");
    }

    void musicValueSet(float value) {
        if(GameObject.FindWithTag("PreferencesContainer") != null) {
            GameObject.FindWithTag("PreferencesContainer").GetComponent<PreferencesContainer>().musicLevel = value;
        }
    }

    void effectsValueSet(float value) {
        if(GameObject.FindWithTag("PreferencesContainer") != null) {
            GameObject.FindWithTag("PreferencesContainer").GetComponent<PreferencesContainer>().effectsLevel = value;
        }
    }

    void UpdateSelectedButton() {
        switch(selectedItem) {
            case 1:
                musicSlider.Select();
                musicText.color = selectedColor;
                effectsText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                break;
            case 2:
                effectsSlider.Select();
                musicText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                effectsText.color = selectedColor;
                break;
            default:
                botones[selectedItem].Select();
                Color cleanC = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                musicText.color = cleanC;
                effectsText.color = cleanC;
                break;
        }
    }
}
