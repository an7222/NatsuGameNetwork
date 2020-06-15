using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
using System.Reflection;

class SessionServer : Singleton<SessionServer> {
    int socketId = 1;
    int PORT = 8001;
    Dictionary<int, Socket> connectedSocketPool = new Dictionary<int, Socket>();
    
    public void Start() {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);

        TcpListener listener = new TcpListener(IPAddress.Any, PORT);

        try {
            listener.Start();
            listener.BeginAcceptTcpClient(OnAccept, listener);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("Session Server Start");
    }

    void OnAccept(IAsyncResult ar) {
        Console.WriteLine("Socket Connected");
        TcpListener listener = (TcpListener)ar.AsyncState;
        TcpClient tcpClient = listener.EndAcceptTcpClient(ar);

        TcpClientHandler handler = new TcpClientHandler(tcpClient);
    }
}
