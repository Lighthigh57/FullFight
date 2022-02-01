using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp.Server;

public class WebSocketOSC : MonoBehaviour
{
    public WebSocketServer wssv;
    void Start()
    {
        wssv = new WebSocketServer("ws://127.0.0.1:12345");
        wssv.AddWebSocketService<Echo>("/");
        wssv.Start();
    }
}
