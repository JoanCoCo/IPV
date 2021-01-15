using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public int variants;
    public int variantSize;
    public string[] inputOptionNames;
    public Sprite[] inputOptionImages;

    private string[][] optionNames;
    private Sprite[][] optionImages;
    public Button upArrow;
    public Button downArrow;
    public Text titleDisplay;
    public Button imageDisplay;

    private int selectedOption = 0;
    private int selectedVariant = 0;

    // Start is called before the first frame update
    void Start()
    {
        upArrow.onClick.AddListener(UpClicked);
        downArrow.onClick.AddListener(DownClicked);

        optionNames = new string[variants][];
        optionImages = new Sprite[variants][];
        for (var i = 0; i < variants; i++)
        {
            optionImages[i] = new Sprite[variantSize];
            optionNames[i] = new string[variantSize];
        }
        int v = 0;
        int e = 0;
        for(int i = 0; i < inputOptionNames.Length; i++) {
            optionNames[v][e] = inputOptionNames[i];
            optionImages[v][e] = inputOptionImages[i];
            e++;
            if(e == variantSize) { e = 0; v++; }
        }
    }

    // Update is called once per frame
    void Update()
    {
        titleDisplay.text = optionNames[selectedVariant][selectedOption];
        imageDisplay.GetComponent<Image>().sprite = optionImages[selectedVariant][selectedOption];
        imageDisplay.GetComponent<Image>().preserveAspect = true;
    }

    public string GetSelectedOption() {
        return optionNames[selectedVariant][selectedOption];
    }

    public int GetSelectedOptionIndex() {
        return selectedOption;
    }

    public void SetVariant(int i) {
        selectedVariant = i;
    }

    public void UpClicked() {
        if(selectedOption == 0) {
            selectedOption = optionImages[0].Length - 1;
        } else {
            selectedOption -= 1;
        }
    }

    public void DownClicked() {
        if(selectedOption == optionImages[0].Length - 1) {
            selectedOption = 0;
        } else {
            selectedOption += 1;
        }
    }

    public void DisableOption() {
        imageDisplay.interactable = false;
    }

    public void EnableOption() {
        imageDisplay.interactable =  true;
    }

    public void Highlight() {
        imageDisplay.Select();
    }
}
