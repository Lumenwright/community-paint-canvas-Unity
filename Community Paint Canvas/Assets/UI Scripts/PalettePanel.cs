using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PalettePanel : MonoBehaviour
{
    VisualElement root;

    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        GenerateUI();
    }

    void GenerateUI(){
        Button submitButton = new Button();
        submitButton.RegisterCallback<PointerDownEvent>(OnClick);
        root.Add(submitButton);
    }

    void OnClick(PointerDownEvent e){
        EventSystem.Services.CanvasAPI.Submit();
    }
}
