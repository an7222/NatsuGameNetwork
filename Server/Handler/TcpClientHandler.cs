using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

class TcpClientHandler {
    readonly int BUFFER_SIZE = 1024;
    readonly int PACKET_LENGTH_HEADER_SIZE = 4;
    readonly int PROTOCOL_ID_HEADER_SIZE = 4;

    Byte[] buffer;
    TcpClient tcpClient = null;
    NetworkStream stream = null;
    public TcpClientHandler(TcpClient tcpClient) {
        this.tcpClient = tcpClient;
        this.stream = tcpClient.GetStream();
        buffer = new Byte[BUFFER_SIZE];

        Start();
    }

    int currentBufferIndex = 0;
    void Start() {
        ReceivePacket(PACKET_LENGTH_HEADER_SIZE);

        int packetLength = 0;
        using (BinaryReader br = new BinaryReader(stream)) {
            packetLength = br.ReadInt32();
            Console.WriteLine(packetLength);
        }

        stream.Flush();

        ReceivePacket(packetLength);

        using (BinaryReader br = new BinaryReader(stream)) {
            int protocol_id = br.ReadInt32();
            Console.WriteLine(protocol_id);

            var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
            protocol.Read(br);
            ProtocolHandler.GetInstance().ProtocolHandle(protocol);
        }

        stream.Flush();


        Start();


        //foreach (var connectedSocket in connectedSocketPool.Values) {
        //    // Echo the data back to the client.  
        //    Send(connectedSocket, content);
        //}
    }

    async void ReceivePacket(int packet_size) {
        int bytesReceived = await stream.ReadAsync(buffer, 0, packet_size);

        while (bytesReceived >= packet_size) {
            bytesReceived += await stream.ReadAsync(buffer, 0, packet_size - bytesReceived);
        }
    }

    void sendPacket(IProtocol protocol) {
        using(BinaryWriter bw = new BinaryWriter(stream)) {
            protocol.Write(bw);
        }

        stream.WriteAsync(buffer);
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