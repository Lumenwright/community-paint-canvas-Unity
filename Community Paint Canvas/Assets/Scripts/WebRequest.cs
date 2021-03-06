using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Newtonsoft.Json;
using System.Text;

namespace JsonClasses{
public class WebRequest :MonoBehaviour
{
    public List<List<PixelData>> CanvasData {get=>ConvertFromJSON();}
    string _canvasData;
    string PIXELS_ENDPOINT = "http://127.0.0.1:5000/pixels";
    string NEW_PX_NAME = "new_pixels";
    string CANVAS_NAME = "Canvas";
    string ROW_NAME = "Px_Row";
    string PX_NAME = "Px";
    string X_NAME = "x";
    string Y_NAME = "y";
    string RED_NAME = "r";
    string GREEN_NAME = "g";
    string BLUE_NAME = "b";

    public UnityEvent m_CanvasChanged;
    void OnEnable(){
        Debug.Log("web");
        if (m_CanvasChanged == null)
            m_CanvasChanged = new UnityEvent();
    }

    // Start is called before the first frame update
    public void Get()
    {
        if(enabled)
            StartCoroutine(GetRequest(PIXELS_ENDPOINT));
    }

    /// submit a change to a single pixel
    public void Post(string x, string y, Px newPixel){

        Dictionary<string,string> p = new Dictionary<string,string>();
        p.Add(X_NAME, x);
        p.Add(Y_NAME, y);
        p.Add(RED_NAME, newPixel.r.ToString());
        p.Add(GREEN_NAME, newPixel.g.ToString());
        p.Add(BLUE_NAME, newPixel.b.ToString());

        if(enabled)
            StartCoroutine(Upload(PIXELS_ENDPOINT, p));
    }

    /// submit a change to a list of pixels
    public void Post(PixelSubmission s){
        if(enabled){
            string c = JsonConvert.SerializeObject(s);
            Debug.Log($"uploading: {c}");
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add(NEW_PX_NAME, c);
            StartCoroutine(Upload(PIXELS_ENDPOINT, d));
        }
    }

    public void Submit(){
        var list = EventSystem.Services.CanvasUI.ChangedPixels;
        var total = EventSystem.Services.MainPanel.Total;
        var text = EventSystem.Services.MainPanel.Response;
        PixelSubmission sub = new PixelSubmission(list, total, text);
        Post(sub);
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    _canvasData = webRequest.downloadHandler.text;
                    m_CanvasChanged.Invoke();
                    break;
            }
        }
    }

    /// send a dictionary expecting keys X, Y, red, green, blue
    /// but it can work for anything to anywhere
    IEnumerator Upload(string uri, Dictionary<string,string> p)
    {

        using (UnityWebRequest www = UnityWebRequest.Post(uri,p))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log($"{www.error} when uploading: {p}");
            }
            else
            {
                Debug.Log($"Completed upload:{p}");
                Get();
            }
        }
    }

//DOESNT WORK
/*
    IEnumerator Upload(string uri, string p)
    {
        var bytejson = Encoding.UTF8.GetBytes(p);
        UnityWebRequest www = new UnityWebRequest(uri,"POST");
        www.uploadHandler = (UploadHandler) new UploadHandlerRaw(bytejson);
        
        www.SetRequestHeader("Content-Type","application/json");
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log($"{www.error} when uploading: {p}");
        }
        else
        {
            Debug.Log($"Completed upload:{p}");
            Get();
        }
        
    }
    */

    List<List<PixelData>> ConvertFromJSON(){
//deserialize
        Canvas list = JsonConvert.DeserializeObject<Canvas>(_canvasData);
        List<List<PixelData>> pixels = new List<List<PixelData>>();
        foreach(Px_Row row in list.canvas){
            List<PixelData> pRow = new List<PixelData>();
            foreach(Px_Container p in row.px_row){
                pRow.Add(new PixelData(){
                    x=int.Parse(p.px.x),
                    y=int.Parse(p.px.y),
                    r=float.Parse(p.px.r),
                    g=float.Parse(p.px.g),
                    b=float.Parse(p.px.b)});
            }
            pixels.Add(pRow);
        }
        return pixels;
    }
}

[Serializable]
public class Canvas{
    public List<Px_Row> canvas;
    public float total_donate;
    public Canvas(){
        canvas = new List<Px_Row>();
    }
}

[Serializable]
public class Px_Row{
    public List<Px_Container> px_row;

    public Px_Row(){
        px_row = new List<Px_Container>();
    }
}

[Serializable]
public class Px_Container{
    public Px px;
}

[Serializable]
public class Px{
    public string x;
    public string y;
    public string r;
    public string g;
    public string b;
}

public class PixelSubmission{
    public List<Px_Container> new_pixels;
    public float total_donate;
    public string response;
    public PixelSubmission(List<PixelData> list, float total, string text){
        List<Px_Container> newList = new List<Px_Container>();
        for(int i=0; i<list.Count; i++){
            var p = list[i];
            newList.Add(new Px_Container()
                {px = new Px(){
                x=p.x.ToString(),
                y=p.y.ToString(),
                r=p.r.ToString(),
                g=p.g.ToString(),
                b=p.b.ToString()
            }});
        }
        new_pixels = newList;
        total_donate = total;
        response = text;
    }
}
}
public class PixelData{
    public int x;
    public int y;
    public float r;
    public float g;
    public float b;
}