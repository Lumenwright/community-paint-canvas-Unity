using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// script to enforce start order
public class Startup : MonoBehaviour
{
    public EventSystem Services;
    void Awake(){
        Debug.Log("startup");
        Services.enabled = true;
        Services.CanvasAPI.enabled = true;
        Services.CanvasAPI.Get();
        Services.CanvasUI.enabled = true;
        Services.MainPanel.enabled = true;
    }
}
