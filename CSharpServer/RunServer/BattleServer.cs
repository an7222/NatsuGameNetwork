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
    Dictionary<int, TcpHandler> connectedClientPool = new Dictionary<int, TcpHandler>();
    Dictionary<int, ZoneController> zoneControllerPool = new Dictionary<int, ZoneController>();

    public void Start() {
        var fieldDataList = new List<Field_Excel>();
        //TODO : Read for DB or Excel
        fieldDataList.Add(new Field_Excel {
            FIELD_ID = 1,
            FieldName = "안토리네 집",
        });
        //fieldDataList.Add(new Field_Excel {
        //    FIELD_ID = 2,
        //    FieldName = "깃허브",
        //});

        for (int i = 0; i < fieldDataList.Count; ++i) {
            ZoneController zoneController = new ZoneController();
            zoneControllerPool.Add(fieldDataList[i].FIELD_ID, zoneController);
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

        TcpHandler_Battle handler = new TcpHandler_Battle(tcpClient, SESSION_ID, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void AddClient(TcpHandler client) {
        if (connectedClientPool.TryAdd(SESSION_ID, client)) {
            SESSION_ID = Interlocked.Increment(ref SESSION_ID);
        }

        if (client is TcpHandler_Battle) {
            var castClient = client as TcpHandler_Battle;

            ZoneController zoneCon;
            if (zoneControllerPool.TryGetValue(castClient.ZONE_ID, out zoneCon)) {
                zoneCon.AddClient(castClient);
            }
        }
    }

    public void RemoveClient(int session_id) {
        TcpHandler client;
        if (connectedClientPool.TryGetValue(session_id, out client)) {
            if (client is TcpHandler_Battle) {
                var castClient = client as TcpHandler_Battle;
                ZoneController zoneCon;
                if (zoneControllerPool.TryGetValue(castClient.ZONE_ID, out zoneCon)) {
                    zoneCon.RemoveClient(castClient);
                }
            }
        }
        connectedClientPool.Remove(session_id);
    }

    public void SendPacketToZone(IProtocol protocol, int zone_id) {
        ZoneController zoneCon;

        if (zoneControllerPool.TryGetValue(zone_id, out zoneCon)) {
            zoneCon.SendPacketToZone(protocol);
        } else {
            Console.WriteLine("No Field!");
        }
    }

    public IEnumerable<ZoneController> GetZoneControllerPool() {
        return zoneControllerPool.Values;
    }
}
