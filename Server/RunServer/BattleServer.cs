using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

class BattleServer : Singleton<BattleServer>, IRealTimeServer {
    int session_id = 1;
    Dictionary<int, TcpSessionHandler> connectedClientPool = new Dictionary<int, TcpSessionHandler>();
    Dictionary<int, FieldController> fieldControllerPool = new Dictionary<int, FieldController>();

    public void Start() {
        var fieldDataList = new List<Field_Excel>();
        //TODO : Read for DB or Excel
        fieldDataList.Add(new Field_Excel {
            FieldId = 1,
            FieldName = "안토리네 집",
        });
        fieldDataList.Add(new Field_Excel {
            FieldId = 2,
            FieldName = "깃허브",
        });

        for (int i = 0; i < fieldDataList.Count; ++i) {
            FieldController fieldController = new FieldController();
            fieldControllerPool.Add(fieldDataList[0].FieldId, fieldController);
        }

        TcpListener listener = new TcpListener(IPAddress.Any, Const.BATTLE_SERVER_PORT);

        try {
            listener.Start();
            listener.BeginAcceptTcpClient(OnAccept, listener);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("Battle Server Start");
    }

    void OnAccept(IAsyncResult ar) {
        Console.WriteLine("Client Connected");
        TcpListener listener = (TcpListener)ar.AsyncState;
        TcpClient tcpClient = listener.EndAcceptTcpClient(ar);

        TcpSessionHandler_Battle handler = new TcpSessionHandler_Battle(tcpClient, session_id, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void AddClient(TcpSessionHandler client) {
        if (connectedClientPool.TryAdd(session_id, client)) {
            session_id = Interlocked.Increment(ref session_id);
        }

        FieldController fieldCon;
        if (fieldControllerPool.TryGetValue(client.GetFieldId(), out fieldCon)) {
            fieldCon.AddClient(client);
        }
    }

    public void RemoveClient(int session_id) {
        TcpSessionHandler handler;
        if (connectedClientPool.TryGetValue(session_id, out handler)) {
            FieldController fieldCon;
            if (fieldControllerPool.TryGetValue(handler.GetFieldId(), out fieldCon)) {
                fieldCon.RemoveClient(handler);
            }
        }
        connectedClientPool.Remove(session_id);
    }

    public void SendPacketField(IProtocol protocol, int field_id) {
        FieldController fieldCon;

        if (fieldControllerPool.TryGetValue(field_id, out fieldCon)) {
            fieldCon.SendPacketField(protocol);
        } else {
            Console.WriteLine("No Field!");
        }
    }
    
    public IEnumerable<FieldController> GetFieldControllerPool() {
        return fieldControllerPool.Values;
    }
}
