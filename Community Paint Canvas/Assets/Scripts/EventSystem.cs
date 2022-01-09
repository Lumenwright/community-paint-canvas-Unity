using UnityEngine.Events;
using UnityEngine;
public class EventSystem : MonoBehaviour
{
    public static EventSystem Services;
    public JsonClasses.WebRequest CanvasAPI;
    public PaintCanvas CanvasUI;

    void Awake(){
        if(Services != null){
            Destroy(this);
        }
        else{
            Services = this;
        }
    }
}
