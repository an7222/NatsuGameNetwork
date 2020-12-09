using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

class TcpHandler_Battle : TcpHandler{
    public int ZONE_ID {
        get;set;
    }
    public PlayerCharacter PlayerCharacter {
        get;set;
    }

    public TcpHandler_Battle(TcpClient tcpClient, int session_id, IRealTimeServer connectedServer) : base(tcpClient, session_id, connectedServer) {
    }
}
