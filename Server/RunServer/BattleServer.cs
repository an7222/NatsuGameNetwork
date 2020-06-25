using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

class BattleServer : Singleton<BattleServer>, IRealTimeServer {
    int SESSION_ID = 1;
    Dictionary<int, TcpSessionHandler> connectedClientPool = new Dictionary<int, TcpSessionHandler>();
    Dictionary<int, FieldController> fieldControllerPool = new Dictionary<int, FieldController>();

    public void Start() {
        var fieldDataList = new List<Field_Excel>();
        //TODO : Read for DB or Excel
        fieldDataList.Add(new Field_Excel {
            FIELD_ID = 1,
            FieldName = "안토리네 집",
        });
        fieldDataList.Add(new Field_Excel {
            FIELD_ID = 2,
            FieldName = "깃허브",
        });

        for (int i = 0; i < fieldDataList.Count; ++i) {
            FieldController fieldController = new FieldController();
            fieldControllerPool.Add(fieldDataList[i].FIELD_ID, fieldController);
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

        TcpSessionHandler_Battle handler = new TcpSessionHandler_Battle(tcpClient, SESSION_ID, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void AddClient(TcpSessionHandler client) {
        if (connectedClientPool.TryAdd(SESSION_ID, client)) {
            SESSION_ID = Interlocked.Increment(ref SESSION_ID);
        }

        if (client is TcpSessionHandler_Battle) {
            var castClient = client as TcpSessionHandler_Battle;

            FieldController fieldCon;
            if (fieldControllerPool.TryGetValue(castClient.FIELD_ID, out fieldCon)) {
                fieldCon.AddClient(castClient);
            }
        }
    }

    public void RemoveClient(int session_id) {
        TcpSessionHandler client;
        if (connectedClientPool.TryGetValue(session_id, out client)) {
            if (client is TcpSessionHandler_Battle) {
                var castClient = client as TcpSessionHandler_Battle;
                FieldController fieldCon;
                if (fieldControllerPool.TryGetValue(castClient.FIELD_ID, out fieldCon)) {
                    fieldCon.RemoveClient(castClient);
                }
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
