using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

/// The UI Pixel
public class Pixel : VisualElement
{

    Color _activeColour = Color.black;
    Color _previousColour;

    public int X {get=>_x;}
    int _x;
    public int Y {get=>_y;}
    int _y;

    public bool IsActive {get=>_isActive;}
    bool _isActive = false;

    PixelData _pixelObj;
    public Pixel(){
    }
    public void Init(PixelData p){
        _x = p.x;
        _y = p.y;
        _pixelObj = p;
        AddToClassList("pixel-container");
        VisualTreeAsset _pixelTemplate = Resources.Load<VisualTreeAsset>("Pixel_Template");
        _pixelTemplate.CloneTree(this);
        this.Q<Button>().RegisterCallback<PointerDownEvent>(OnPointerDown);

        //change bg colour
        _previousColour = new Color(p.r, p.g, p.b); 
        ChangeColour(_previousColour);
    }

    void OnPointerDown(PointerDownEvent evt){
        Debug.Log("clicked");
        _isActive = !_isActive;
        if(_isActive){
            _pixelObj.r = _activeColour.r;
            _pixelObj.g = _activeColour.g;
            _pixelObj.b = _activeColour.b;
            EventSystem.Services.CanvasUI.AddPixel(_pixelObj);
            ChangeColour(_activeColour);
        }
        else{
            _pixelObj.r = _previousColour.r;
            _pixelObj.g = _previousColour.g;
            _pixelObj.b = _previousColour.b;
            EventSystem.Services.CanvasUI.RemovePixel(_pixelObj);
            ChangeColour(_previousColour);
        }

    }

    void ChangeColour(Color color){
        this.Q<Button>().style.backgroundColor = new StyleColor(color);
    }
}

     #region UXML
        [Preserve]
        public class PixelDataFactory : UxmlFactory<Pixel, PixelDataTraits> { }

        [Preserve]
        public class PixelDataTraits : VisualElement.UxmlTraits { }
        #endregion
