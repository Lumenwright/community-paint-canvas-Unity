using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PaintCanvas : MonoBehaviour
{
    VisualElement root;

    VisualTreeAsset _pixelTemplate;

    public List<PixelData> ChangedPixels {get=>_changedPixels;}
    List<PixelData> _changedPixels;

    // Start is called before the first frame update
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("canvas");
        EventSystem.Services.CanvasAPI.m_CanvasChanged.AddListener(GenerateCanvas);
        ResetSubmission();
    }

    // Add a changed pixel information to the submission.
    public List<PixelData> AddPixel(PixelData newPx){
        if(_changedPixels!=null){
            _changedPixels.Add(newPx);
            EventSystem.Services.MainPanel.UpdateTotal(_changedPixels.Count);
        }
        return _changedPixels;
    }

    public List<PixelData> RemovePixel(PixelData px){
        if(_changedPixels!=null){
            _changedPixels.Remove(px);
            EventSystem.Services.MainPanel.UpdateTotal(_changedPixels.Count);
        }
        return _changedPixels;
    }

    public void ResetSubmission(){
        _changedPixels = new List<PixelData>();
    }

    // Get the canvas from the API and generate the UI
    void GenerateCanvas(){
        if(!enabled){
            Debug.Log("CanvasController not enabled.");
            return;
        }

        JsonClasses.WebRequest onlineCanvas = EventSystem.Services.CanvasAPI;
        List<List<PixelData>> canvas = onlineCanvas.CanvasData;
        var lenX = canvas.Count;
        for(int i = 0; i<lenX; i++){
            Row newRow = new Row();
            var lenY = canvas[i].Count;
            for(int j =0; j<lenY; j++){
                Pixel newPx = new Pixel();
                newPx.Init(canvas[i][j]);
                var container = newRow.Q<VisualElement>("RowElement");
                container.Add(newPx);
            }
            root.Add(newRow);
        }
        onlineCanvas.m_CanvasChanged.RemoveListener(GenerateCanvas);
        onlineCanvas.m_CanvasChanged.AddListener(OnClick);
    }

    void OnClick(){
            Debug.Log("The canvas was refreshed");
    }
}