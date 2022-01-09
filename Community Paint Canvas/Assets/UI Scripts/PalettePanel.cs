using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PalettePanel : MonoBehaviour
{
    //params
    [SerializeField] float _pxToPriceFactor = 100f; //cents

    // UI elements
    VisualElement _root;
    Label _total_element;
    TextField _response_element;
    Label _confirm_element;

    //internal
    public float Total {get=>_total;}
    float _total; // dollars.cents
    public string Response {get=>_response;}
    string _response;

    // Start is called before the first frame update
    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("palette-panel");
        _total_element= _root.Q<VisualElement>("Counter").Q<Label>("total-number");
        _response_element = _root.Q<TextField>("response-field");
        _confirm_element = _root.Q<Label>("confirmation");
        GenerateUI();
    }

    public void UpdateTotal(int numPxTotal){
        _total = numPxTotal*_pxToPriceFactor/100;
        _total_element.text = $"${_total}";
    }

    // update from text field
    public void UpdateResponse(){
        _response = _response_element.text;
    }

    void GenerateUI(){
        Button submitButton = _root.Q<Button>("submit-button");
        submitButton.RegisterCallback<PointerDownEvent>(OnClick);
    }

    void OnClick(PointerDownEvent e){
        UpdateResponse();
        EventSystem.Services.CanvasAPI.Submit();
        _confirm_element.visible = true;
    }
}
