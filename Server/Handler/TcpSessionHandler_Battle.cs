using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

class TcpSessionHandler_Battle : TcpSessionHandler{
    int field_id = 0;
    FieldController fc;

    public TcpSessionHandler_Battle(TcpClient tcpClient, int session_id, IRealTimeServer connectedServer) : base(tcpClient, session_id, connectedServer) {
    }

    public void SetFieldId(int field_id) {
        this.field_id = field_id;
    }
    public int GetFieldId() {
        return field_id;
    }

    public void SetFieldController(FieldController fc) {
        this.fc = fc;
    }

    public FieldController GetFieldController() {
        return fc;
    }
}
