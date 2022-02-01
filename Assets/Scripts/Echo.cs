using UnityEngine;
using VVVV_OSC;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Echo : WebSocketBehavior
{
    //receive message
    protected override void OnMessage(MessageEventArgs e)
    {
        if (e.IsBinary)
        {
            OSCPacket msg = OSCPacket.Unpack(e.RawData);
            Debug.Log(string.Format("recv OSC addr: {0}, value: {1}", msg.Address, msg.Values[0]));
        }
    }
    protected override void OnOpen()
    {
        //send message
        Send(new OSCMessage("/unity/midi", 87).BinaryData);
    }
}
