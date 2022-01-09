using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public EventSystem Services;
    void Awake(){
        Services.CanvasAPI.enabled = true;
        Services.CanvasAPI.Get();
        Services.CanvasUI.enabled = true;
    }
}
