using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class Pixel : VisualElement
{
    public Pixel(){
        AddToClassList("pixel-container");
        VisualTreeAsset _pixelTemplate = Resources.Load<VisualTreeAsset>("Pixel_Template");
        _pixelTemplate.CloneTree(this);
        this.Q<Button>().RegisterCallback<PointerDownEvent>(OnPointerDown);
    }

    void OnPointerDown(PointerDownEvent evt){
        Debug.Log("clicked");
        WebRequest.CanvasAPI.Get();
    }
}

     #region UXML
        [Preserve]
        public class PxFactory : UxmlFactory<Pixel, PxTraits> { }

        [Preserve]
        public class PxTraits : VisualElement.UxmlTraits { }
        #endregion
