using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;

class SessionServer : Singleton<SessionServer> {
    Dictionary<string, Socket> connectedSocketPool = new Dictionary<string, Socket>();
    public void Start() {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8001);

        Socket listenSocket = new Socket(endPoint.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        try {
            listenSocket.Bind(endPoint);
            listenSocket.Listen(10);

            listenSocket.BeginAccept(new AsyncCallback(OnAccept), listenSocket);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

    }

    void OnAccept(IAsyncResult ar) {
        Socket listenSocket = (Socket)ar.AsyncState;
        Socket workSocket = listenSocket.EndAccept(ar);

        // Create the state object.  
        StateObject state = new StateObject();
        state.WorkSocket = workSocket;
        workSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(OnRead), state);

        listenSocket.BeginAccept(new AsyncCallback(OnAccept), listenSocket);
    }

    void OnRead(IAsyncResult ar) {
        StateObject state = (StateObject)ar.AsyncState;
        Socket workSocket = state.WorkSocket;

        int read = workSocket.EndReceive(ar);

        if (read > 0) {
            state.sb.Append(Encoding.ASCII.GetString(state.Buffer, 0, read));
            workSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(OnRead), state);
        } else {
            if (state.sb.Length > 1) {
                string content = state.sb.ToString();
                Console.WriteLine($"Read {content.Length} bytes from socket.\n Data : {content}");

                if (connectedSocketPool.TryAdd(content, workSocket)) {
                    //Success
                } else {
                    //Already Exist
                }
            }
        }
    }

    public class StateObject {
        public Socket WorkSocket = null;
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
