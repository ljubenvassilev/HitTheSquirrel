using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Vuforia;

public class GameManagerScript : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject[] heads, buttons;
    public GameObject scoreObject;
    private TextMesh text;
    private static int activeTarget, score;
    private static string url = "http://35.193.126.9/";
    private static bool locked;

    void Start () {
        text = scoreObject.GetComponent<TextMesh>();
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        }

        StartGame();
	}

    private void StartGame()
    {
        locked = true;
        UnityWebRequest start = UnityWebRequest.Get(url);
        start.SetRequestHeader("start", "true");
        StartCoroutine(WaitForRequest(start));
    }

    IEnumerator WaitForRequest(UnityWebRequest www)
    {
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            text.text = www.error;
        }
        else
        {
            JSONObject response = new JSONObject(www.downloadHandler.text, -2, false, false);
            string sc, pos;
            response.GetField(out sc, "score", "0");
            text.text = sc;
            response.GetField(out pos, "position", "0");
            activeTarget = Int32.Parse(pos);
            heads[activeTarget - 1].SetActive(true);
        }
        locked = false;
    }

    private void Score()
    {
        UnityWebRequest score = UnityWebRequest.Get(url);
        score.SetRequestHeader("score", text.text);
        StartCoroutine(WaitForRequest(score));
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        if (!locked)
        {
            heads[activeTarget - 1].SetActive(false);
            if (Int32.Parse(vb.VirtualButtonName) == activeTarget)
            {
                Score();
            }
            else
            {
                StartGame();
            }
        }
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb) {}
}
