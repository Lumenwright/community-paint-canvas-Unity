using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class Pixel : VisualElement
{
    public string X {get=>_x;}
    string _x;
    public string Y {get=>_y;}
    string _y;
    public Pixel(){
    }
    public void Init(string x, string y){
        _x = x;
        _y = y;
        AddToClassList("pixel-container");
        VisualTreeAsset _pixelTemplate = Resources.Load<VisualTreeAsset>("Pixel_Template");
        _pixelTemplate.CloneTree(this);
        this.Q<Button>().RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    void OnPointerDown(PointerDownEvent evt){
        Debug.Log("clicked");
        WebRequest.CanvasAPI.Post(X, Y);
    }
}

     #region UXML
        [Preserve]
        public class PxFactory : UxmlFactory<Pixel, PxTraits> { }

        [Preserve]
        public class PxTraits : VisualElement.UxmlTraits { }
        #endregion
