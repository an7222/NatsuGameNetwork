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
        var zoneDataList = new List<Zone_Excel>();
        //TODO : Read for DB or Excel
        for(int i = 0; i < 10; ++i) {
            zoneDataList.Add(new Zone_Excel {
                ZONE_ID= i,
                ZONE_NAME = "임시 존",
            });
        }

        for (int i = 0; i < zoneDataList.Count; ++i) {
            ZoneController zoneController = new ZoneController();
            if(false == zoneControllerPool.TryAdd(zoneDataList[i].ZONE_ID, zoneController)) {
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
            Console.WriteLine("No Zone!");
        }
    }

    public IEnumerable<ZoneController> GetZoneControllerPool() {
        return zoneControllerPool.Values;
    }
}
