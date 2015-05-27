using UnityEngine;
using System.Collections;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

using UnityEditor;
public class DisconnectButton : MonoBehaviour {


    public SynchronousClient camera_sc;

    public void Disconnect()
    {
        byte[] msg = Encoding.ASCII.GetBytes(camera_sc.PlayerIndex.ToString() + "," + "X" +"," + "<EOF>");
        camera_sc.synch_client.sender.Send(msg);
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
