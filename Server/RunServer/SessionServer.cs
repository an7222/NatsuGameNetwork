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
    int session_id = 1;
    long user_id = 10000;
    Dictionary<int, TcpClientHandler> connectedTcpClientPool = new Dictionary<int, TcpClientHandler>();
    
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

        TcpClientHandler handler = new TcpClientHandler(tcpClient, session_id, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void SendPacketAll(IProtocol protocol) {
        foreach(var handler in connectedTcpClientPool.Values) {
            handler.SendPacket(protocol);
        }
    }

    public void OnClientLeave(int session_id) {
        connectedTcpClientPool.Remove(session_id);
    }

    public void OnLoginComplete(TcpClientHandler handler) {
        if (connectedTcpClientPool.TryAdd(session_id, handler)) {
            session_id = Interlocked.Increment(ref session_id);
        }
    }

    public long GetUserID() {
        return user_id = Interlocked.Increment(ref user_id);
    }
}
