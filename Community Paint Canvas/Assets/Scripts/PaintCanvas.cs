using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PaintCanvas : MonoBehaviour
{
    VisualElement root;
    public WebRequest _onlineCanvas;

    VisualTreeAsset _pixelTemplate;

    // Start is called before the first frame update
    void OnEnable()
    {
        _onlineCanvas.m_CanvasChanged.AddListener(GenerateCanvas);
        root = GetComponent<UIDocument>().rootVisualElement;
    }

    void GenerateCanvas(){
        Debug.Log("The canvas was refreshed");
        for(int i = 0; i<5; i++){
            Row newRow = new Row();
            for(int j =0; j<5; j++){
                Pixel newPx = new Pixel();
                var container = newRow.Q<VisualElement>("RowElement");
                container.Add(newPx);
            }
            root.Add(newRow);
        }
    }
}