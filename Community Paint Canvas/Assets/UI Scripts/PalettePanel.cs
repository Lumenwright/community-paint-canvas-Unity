using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PalettePanel : MonoBehaviour
{
    //params
    [SerializeField] float _pxToPriceFactor = 1f;

    // internal
    VisualElement root;
    VisualElement total_element;
    float _total;

    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("palette-panel");
        total_element = root.Q<VisualElement>("Counter").Q<Label>("total-number");
        GenerateUI();
    }

    public void UpdateTotal(int numPx){

    }

    void GenerateUI(){
        Button submitButton = root.Q<Button>("submit-button");
        submitButton.RegisterCallback<PointerDownEvent>(OnClick);
    }

    void OnClick(PointerDownEvent e){
        EventSystem.Services.CanvasAPI.Submit();
    }
}
