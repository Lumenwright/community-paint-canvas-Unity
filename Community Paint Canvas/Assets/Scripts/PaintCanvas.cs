using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PaintCanvas : MonoBehaviour
{
    VisualElement root;
    public WebRequest _onlineCanvas;

    VisualTreeAsset _pixelTemplate;

    public static PaintCanvas CanvasController;
    void Awake(){
        if(CanvasController!=null){
            Destroy(this);
        }
        else{
            CanvasController = this;
        }
        enabled = false;
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        WebRequest.CanvasAPI.Get();
        _onlineCanvas.m_CanvasChanged.AddListener(GenerateCanvas);
    }

    void GenerateCanvas(){
        List<List<Px>> canvas = _onlineCanvas.CanvasData;
        var lenX = canvas.Count;
        var lenY = canvas[0].Count;
        for(int i = 0; i<lenX; i++){
            Row newRow = new Row();
            for(int j =0; j<lenY; j++){
                Pixel newPx = new Pixel();
                newPx.Init(i.ToString(),j.ToString());
                var container = newRow.Q<VisualElement>("RowElement");
                container.Add(newPx);
            }
            root.Add(newRow);
        }
        _onlineCanvas.m_CanvasChanged.RemoveListener(GenerateCanvas);
        _onlineCanvas.m_CanvasChanged.AddListener(OnClick);

    }

    void OnClick(){
            Debug.Log("The canvas was refreshed");
    }
}