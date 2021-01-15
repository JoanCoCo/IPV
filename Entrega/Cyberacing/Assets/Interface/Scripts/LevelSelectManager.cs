using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public Button correr;
    public Button salir;
    public Selector[] selectors;
    private enum selected {League, Level, Car};

    private int focusObject;
    private enum focusable {BackButton, SelectorLeague, SelectorLevel, SelectorCar, RunButton};

    private bool keyPressed;

    // Start is called before the first frame update
    void Start()
    {
        correr.onClick.AddListener(correrClicked);
        salir.onClick.AddListener(salirClicked);
        focusObject = (int)focusable.RunButton;
        UpdateFocusedObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) {
            salirClicked();
        }

        if(Input.GetKey(KeyCode.Return) && focusObject == (int)focusable.RunButton && correr.interactable == true) {
            correrClicked();
        }

        if(Input.GetKey(KeyCode.Return) && focusObject == (int)focusable.BackButton) {
            salirClicked();
        }

        selectors[(int)selected.Level].SetVariant(selectors[(int)selected.League].GetSelectedOptionIndex());

        if(selectors[(int)selected.League].GetSelectedOptionIndex() != 0) {
            selectors[(int)selected.League].DisableOption();
        } else {
            selectors[(int)selected.League].EnableOption();
        }

        if(selectors[(int)selected.Level].GetSelectedOptionIndex() != 0 
            || selectors[(int)selected.League].GetSelectedOptionIndex() != 0) {
            selectors[(int)selected.Level].DisableOption();
        } else {
            selectors[(int)selected.Level].EnableOption();
        }

        if(selectors[(int)selected.Car].GetSelectedOptionIndex() != 0) {
            selectors[(int)selected.Car].DisableOption();
        } else {
            selectors[(int)selected.Car].EnableOption();
        }

        if(selectors[(int)selected.Car].GetSelectedOptionIndex() == 0 
            && selectors[(int)selected.Level].GetSelectedOptionIndex() == 0
            && selectors[(int)selected.League].GetSelectedOptionIndex() == 0) {
            correr.interactable = true;
        } else {
            correr.interactable = false;
        }

        if(Input.GetKey(KeyCode.UpArrow) && !keyPressed && focusObject != (int)focusable.RunButton && focusObject != (int)focusable.BackButton) {
            keyPressed = true;
            selectors[focusObject-1].UpClicked();
        }

        if(Input.GetKey(KeyCode.DownArrow) && !keyPressed && focusObject != (int)focusable.RunButton && focusObject != (int)focusable.BackButton) {
            keyPressed = true;
            selectors[focusObject-1].DownClicked();
        }

        if(Input.GetKey(KeyCode.RightArrow) && !keyPressed) {
            keyPressed = true;
            focusObject++;
            if(focusObject > (int)focusable.RunButton) {
                focusObject = 0;
            }
            UpdateFocusedObject();
        }

        if(Input.GetKey(KeyCode.LeftArrow) && !keyPressed) {
            keyPressed = true;
            focusObject--;
            if(focusObject < 0) {
                focusObject = (int)focusable.RunButton;
            }
            UpdateFocusedObject();
        }

        if((Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) 
            || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && keyPressed) {
            keyPressed = false;
        }
    }

    void correrClicked() {
        int level = (selectors[(int)selected.League].GetSelectedOptionIndex() + 1) 
            * (selectors[(int)selected.Level].GetSelectedOptionIndex() + 1);
        SceneManager.LoadScene("Level" + level.ToString());
    }

    void salirClicked() {
        SceneManager.LoadScene("MenuPrincipal");
    }

    void UpdateFocusedObject() {
        switch (focusObject)
        {
            case (int)focusable.RunButton:
                correr.Select();
                break;
            case (int)focusable.SelectorLeague:
                selectors[(int)selected.League].Highlight();
                break;
            case (int)focusable.SelectorLevel:
                selectors[(int)selected.Level].Highlight();
                break;
            case (int)focusable.SelectorCar:
                selectors[(int)selected.Car].Highlight();
                break;
            case (int)focusable.BackButton:
                salir.Select();
                break;
            default:
                break;
        }
        
    }
}
