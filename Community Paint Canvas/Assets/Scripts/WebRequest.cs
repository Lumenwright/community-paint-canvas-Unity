using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

public class WebRequest : MonoBehaviour
{
    public string CanvasData {get=>_canvasData;}
    string _canvasData;
    public UnityEvent m_CanvasChanged;

    public static WebRequest CanvasAPI;

    void Awake(){
        if(CanvasAPI != null){
            Destroy(this);
        }
        else{
            CanvasAPI = this;
        }
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
        StartCoroutine(GetRequest("http://127.0.0.1:5000/pixels"));
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
}
