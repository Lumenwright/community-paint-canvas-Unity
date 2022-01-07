using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using Newtonsoft.Json;

public class WebRequest : MonoBehaviour
{
    public List<List<Px>> CanvasData {get=>ConvertJSON();}
    string _canvasData;
    public UnityEvent m_CanvasChanged;

    public static WebRequest CanvasAPI;

    string PIXELS_ENDPOINT = "http://127.0.0.1:5000/pixels";

    void Awake(){
        if(CanvasAPI != null){
            Destroy(this);
        }
        else{
            CanvasAPI = this;
        }
        PaintCanvas.CanvasController.enabled = true;
    }

    void Start()
    {
        if (m_CanvasChanged == null)
            m_CanvasChanged = new UnityEvent();
        Get();
    }

    // Start is called before the first frame update
    public void Get()
    {
        StartCoroutine(GetRequest(PIXELS_ENDPOINT));
    }

//TODO: change to json 
    public void Post(string x, string y){
        string X_NAME = "x";
        string Y_NAME = "y";
        string RED = "r";
        string GREEN = "g";
        string BLUE = "b";

        string test = "0";

        Dictionary<string,string> p = new Dictionary<string,string>();
        p.Add(X_NAME, x);
        p.Add(Y_NAME, y);
        p.Add(RED, test );
        p.Add(GREEN, test);
        p.Add(BLUE, test);
        StartCoroutine(Upload(PIXELS_ENDPOINT, p));
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
                Debug.Log("Form upload complete!");
                Get();
            }
        }
    }

    List<List<Px>> ConvertJSON(){
//deserialize
        Canvas list = JsonConvert.DeserializeObject<Canvas>(_canvasData);
        List<List<Px>> pixels = new List<List<Px>>();
        foreach(Px_Row row in list.canvas){
            List<Px> pRow = new List<Px>();
            foreach(Px p in row.px_row){
                pRow.Add(p);
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
    public List<Px> px_row;

    public Px_Row(){
        px_row = new List<Px>();
    }
}

[Serializable]
public class Px{
    public int r;
    public int g;
    public int b;
}