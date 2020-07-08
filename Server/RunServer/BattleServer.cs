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
    Dictionary<int, ChannelController> channelControllerPool = new Dictionary<int, ChannelController>();

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
            ChannelController channelController = new ChannelController();
            channelControllerPool.Add(fieldDataList[i].FIELD_ID, channelController);
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

            ChannelController channelCon;
            if (channelControllerPool.TryGetValue(castClient.CHANNEL_ID, out channelCon)) {
                channelCon.AddClient(castClient);
            }
        }
    }

    public void RemoveClient(int session_id) {
        TcpSessionHandler client;
        if (connectedClientPool.TryGetValue(session_id, out client)) {
            if (client is TcpSessionHandler_Battle) {
                var castClient = client as TcpSessionHandler_Battle;
                ChannelController channelCon;
                if (channelControllerPool.TryGetValue(castClient.CHANNEL_ID, out channelCon)) {
                    channelCon.RemoveClient(castClient);
                }
            }
        }
        connectedClientPool.Remove(session_id);
    }

    public void SendPacketChannel(IProtocol protocol, int channel_id) {
        ChannelController channelCon;

        if (channelControllerPool.TryGetValue(channel_id, out channelCon)) {
            channelCon.SendPacketField(protocol);
        } else {
            Console.WriteLine("No Field!");
        }
    }

    public IEnumerable<ChannelController> GetChannelControllerPool() {
        return channelControllerPool.Values;
    }
}
