using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class BattleServer : Singleton<BattleServer>, IRealTimeServer {
    int session_id = 1;
    Dictionary<int, TcpClientHandler> connectedTcpClientPool = new Dictionary<int, TcpClientHandler>();

    public void Start() {
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

        TcpClientHandler handler = new TcpClientHandler(tcpClient, session_id, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void SendPacketAll(IProtocol protocol) {
        foreach (var handler in connectedTcpClientPool.Values) {
            handler.SendPacket(protocol);
        }
    }

    public void OnClientLeave(int session_id) {
        connectedTcpClientPool.Remove(session_id);
    }
}
