using UnityEngine;
using System.Collections;

public class PhotonConnect : MonoBehaviour {

    public string ServerIP = "ServerIP";
    public int ServerPort = 5055;
    public string ServerName = "ServerName";
    private bool ConnectStatus = true;

	// Use this for initialization
	void Start () {
        if(!PhotonGlobal.PS.ServerConnected)
        {
            PhotonGlobal.PS.ConnectEvent += doConnectEvent;
            PhotonGlobal.PS.Connect(ServerIP, ServerPort, ServerName);
            AnswerGlobal.LoginStatus = false;
        }
	}

    private void doConnectEvent(bool Status)
    {
        if (Status)
        {
            Debug.Log("Connecting . . . . .");
            ConnectStatus = true;
        }
        else
        {
            Debug.Log("Connect Fail");
            ConnectStatus = false;
        }
    }

    private void OnDestroy()
    {
        PhotonGlobal.PS.ConnectEvent -= doConnectEvent;
    }

    void OnGUI()
    {
        if (ConnectStatus == false)
        {
            GUI.Label(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 10, 400, 20), "Connect fail");
        }
    }
}
