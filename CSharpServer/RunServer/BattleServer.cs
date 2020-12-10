using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Concurrent;

class BattleServer : Singleton<BattleServer>, IRealTimeServer {
    int SESSION_ID = 1;
    ConcurrentDictionary<int, TcpHandler> connectedClientPool = new ConcurrentDictionary<int, TcpHandler>();
    ConcurrentDictionary<int, ZoneController> zoneControllerPool = new ConcurrentDictionary<int, ZoneController>();

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
            ZoneController zoneController = new ZoneController();
            if(false == zoneControllerPool.TryAdd(fieldDataList[i].FIELD_ID, zoneController)) {
                Console.WriteLine($"Already Zone? : {i}");
            }
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

            if (zoneControllerPool.TryGetValue(castClient.ZONE_ID, out var zoneCon)) {
                zoneCon.AddClient(castClient);
            }
        }
    }

    public void RemoveClient(int session_id) {
        if (connectedClientPool.TryGetValue(session_id, out var client)) {
            if (client is TcpHandler_Battle) {
                var castClient = client as TcpHandler_Battle;
                if (zoneControllerPool.TryGetValue(castClient.ZONE_ID, out var zoneCon)) {
                    zoneCon.RemoveClient(castClient);
                }
            }
        }
        connectedClientPool.TryRemove(session_id, out _);
    }

    public void SendPacketToZone(IProtocol protocol, int zone_id) {
        if (zoneControllerPool.TryGetValue(zone_id, out var zoneCon)) {
            zoneCon.SendPacketToZone(protocol);
        } else {
            Console.WriteLine("No Field!");
        }
    }

    public IEnumerable<ZoneController> GetZoneControllerPool() {
        return zoneControllerPool.Values;
    }
}
