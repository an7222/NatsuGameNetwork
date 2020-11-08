using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

class TcpSessionHandler_Battle : TcpSessionHandler{
    public int CHANNEL_ID {
        get;set;
    }
    public PlayerCharacter PlayerCharacter {
        get;set;
    }

    public TcpSessionHandler_Battle(TcpClient tcpClient, int session_id, IRealTimeServer connectedServer) : base(tcpClient, session_id, connectedServer) {
    }
}
