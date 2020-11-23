using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
using System.Reflection;

class SessionServer : Singleton<SessionServer>, IRealTimeServer {
    int SESSION_ID = 1;
    long USER_ID = 10000;
    Dictionary<int, TcpHandler> connectedClientPool = new Dictionary<int, TcpHandler>();
    
    public void Start() {
        TcpListener listener = new TcpListener(IPAddress.Any, Const.SESSION_SERVER_PORT);

        try {
            listener.Start();
            listener.BeginAcceptTcpClient(OnAccept, listener);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("Session Server Start");
    }

    void OnAccept(IAsyncResult ar) {
        Console.WriteLine("Client Connected");
        TcpListener listener = (TcpListener)ar.AsyncState;
        TcpClient tcpClient = listener.EndAcceptTcpClient(ar);

        TcpHandler handler = new TcpHandler(tcpClient, SESSION_ID, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void SendPacketAll(IProtocol protocol) {
        foreach(var handler in connectedClientPool.Values) {
            handler.SendPacket(protocol);
        }
    }

    public void AddClient(TcpHandler handler) {
        if (connectedClientPool.TryAdd(SESSION_ID, handler)) {
            SESSION_ID = Interlocked.Increment(ref SESSION_ID);
        }
    }

    public void RemoveClient(int session_id) {
        connectedClientPool.Remove(session_id);
    }

    public long GetUniqueUserID() {
        return USER_ID = Interlocked.Increment(ref USER_ID);
    }
}
