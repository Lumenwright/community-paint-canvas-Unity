using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Newtonsoft.Json;

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
            StartCoroutine(Upload(PIXELS_ENDPOINT, c));
        }
    }

    public void Submit(List<PixelData> list){
        PixelSubmission sub = new PixelSubmission(list);
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
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Single pixel upload complete!");
                Get();
            }
        }
    }

    IEnumerator Upload(string uri, string p)
    {

        using (UnityWebRequest www = UnityWebRequest.Post(uri,p))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("New canvas portion upload complete!");
                Get();
            }
        }
    }

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
    List<Px> new_pixels;

    public PixelSubmission(List<PixelData> list){
        List<Px> newList = new List<Px>();
        for(int i=0; i<list.Count; i++){
            var p = list[i];
            newList.Add(new Px(){
                x=p.x.ToString(),
                y=p.y.ToString(),
                r=p.r.ToString(),
                g=p.g.ToString(),
                b=p.b.ToString()
            });
        }
        new_pixels = newList;
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