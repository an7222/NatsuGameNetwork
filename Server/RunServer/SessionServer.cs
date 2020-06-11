using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.IO;
using System.Threading;

class SessionServer : Singleton<SessionServer> {
    int socketId = 1;
    Dictionary<int, Socket> connectedSocketPool = new Dictionary<int, Socket>();
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


        Console.WriteLine("Session Server Start");
    }

    void OnAccept(IAsyncResult ar) {
        Console.WriteLine("Socket Connected");
        Socket listenSocket = (Socket)ar.AsyncState;
        Socket workSocket = listenSocket.EndAccept(ar);

        // Create the state object.  
        StateObject state = new StateObject();
        state.WorkSocket = workSocket;
        workSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
            new AsyncCallback(OnRead), state);

        listenSocket.BeginAccept(new AsyncCallback(OnAccept), listenSocket);

        if (connectedSocketPool.TryAdd(socketId, workSocket)) {
            //Success
            Interlocked.Increment(ref socketId);
        } else {
            Console.WriteLine("Already Exist");
        }

        foreach (var connectedSocket in connectedSocketPool.Values) {
            // Echo the data back to the client.  
            Send(connectedSocket, "Socket Connected");
        }
    }

    void OnRead(IAsyncResult ar) {
        String content = String.Empty;
        StateObject state = (StateObject)ar.AsyncState;
        Socket workSocket = state.WorkSocket;

        int bytesRead = workSocket.EndReceive(ar);

        if (bytesRead > 0) {
            // There  might be more data, so store the data received so far.  
            state.sb.Append(Encoding.ASCII.GetString(
                state.Buffer, 0, bytesRead));

            // Check for end-of-file tag. If it is not there, read
            // more data.  
            content = state.sb.ToString();
            if (content.IndexOf("<EOF>") > -1) {
                // All the data has been read from the
                // client. Display it on the console.  
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content);


                foreach (var connectedSocket in connectedSocketPool.Values) {
                    // Echo the data back to the client.  
                    Send(connectedSocket, content);
                }

            } else {
                // Not all data received. Get more.  
                workSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(OnRead), state);
            }
        }



    }

    private static void Send(Socket workSocket, String data) {
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        workSocket.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallback), workSocket);
    }

    private static void SendCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.  
            Socket workSocket = (Socket)ar.AsyncState;

            // Complete sending the data to the remote device.  
            int bytesSent = workSocket.EndSend(ar);
            Console.WriteLine("Sent {0} bytes to client.", bytesSent);

        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    public class StateObject {
        public Socket WorkSocket = null;
        public const int BufferSize = 1024;
        public byte[] Buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }
}
