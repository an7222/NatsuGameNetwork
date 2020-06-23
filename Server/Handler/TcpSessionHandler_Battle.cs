using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

class TcpSessionHandler_Battle : TcpSessionHandler{
    int field_id = 0;
    PlayerCharacterController pcc;

    public TcpSessionHandler_Battle(TcpClient tcpClient, int session_id, IRealTimeServer connectedServer) : base(tcpClient, session_id, connectedServer) {
    }

    public void SetFieldId(int field_id) {
        this.field_id = field_id;
    }
    public int GetFieldId() {
        return field_id;
    }

    public void SetPlayerCharacterController(PlayerCharacterController pcc) {
        this.pcc = pcc;
    }

    public PlayerCharacterController GetPlayerCharacterController() {
        return pcc;
    }
}
