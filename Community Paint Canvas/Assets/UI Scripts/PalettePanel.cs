using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PalettePanel : MonoBehaviour
{
    //params
    [SerializeField] float _pxToPriceFactor = 1f; //cents

    // internal
    VisualElement _root;
    Label _total_element;
    public float Total {get=>_total;}
    float _total; // dollars.cents
    public string Response {get=>_response;}
    string _response;

    // Start is called before the first frame update
    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("palette-panel");
        _total_element= _root.Q<VisualElement>("Counter").Q<Label>("total-number");
        GenerateUI();
    }

    public void UpdateTotal(int numPxTotal){
        _total = numPxTotal*_pxToPriceFactor/100;
        _total_element.text = $"${_total}";
    }

    void GenerateUI(){
        Button submitButton = _root.Q<Button>("submit-button");
        submitButton.RegisterCallback<PointerDownEvent>(OnClick);
    }

    void OnClick(PointerDownEvent e){
        EventSystem.Services.CanvasAPI.Submit();
    }
}
