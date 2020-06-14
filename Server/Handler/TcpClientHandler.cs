using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

class TcpClientHandler {
    readonly int BUFFER_SIZE = 400;
    Byte[] buffer;
    TcpClient tcpClient = null;
    public TcpClientHandler(TcpClient tcpClient) {
        this.tcpClient = tcpClient;
        buffer = new Byte[BUFFER_SIZE];

        Start();
    }

    void Start() {
        tcpClient.GetStream().BeginRead(buffer, 0, BUFFER_SIZE, new AsyncCallback(OnRead), tcpClient);
    }

    int currentBufferIndex = 0;
    void OnRead(IAsyncResult ar) {
        TcpClient tcpClient = (TcpClient)ar.AsyncState;
        var stream = tcpClient.GetStream();
        int bytesRead = stream.EndRead(ar);

        currentBufferIndex += bytesRead;
        if (currentBufferIndex > 4) {
            using (BinaryReader br = new BinaryReader(stream)) {
                Console.WriteLine(br.ReadInt32());
            }

            //foreach (var connectedSocket in connectedSocketPool.Values) {
            //    // Echo the data back to the client.  
            //    Send(connectedSocket, content);
            //}

            currentBufferIndex -= 4;
        }

        stream.BeginRead(buffer, 0, BUFFER_SIZE,
new AsyncCallback(OnRead), tcpClient);

    }

//    void Send(Socket workSocket, String data) {
//        // Convert the string data to byte data using ASCII encoding.  
//        byte[] byteData = Encoding.ASCII.GetBytes(data);

//        workSocket.BeginSend(byteData, 0, byteData.Length, 0,
//new AsyncCallback(OnSend), workSocket);
//    }

//    void OnSend(IAsyncResult ar) {
//        // Retrieve the socket from the state object.  
//        // Complete sending the data to the remote device.  
//        int bytesSent = workSocket.EndSend(ar);
//        Console.WriteLine("Sent {0} bytes to client.", bytesSent);
//    }
}