using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

class TcpClientHandler {
    byte[] receiveBuffer;
    byte[] sendBuffer;
    TcpClient tcpClient = null;
    NetworkStream stream = null;
    public TcpClientHandler(TcpClient tcpClient) {
        this.tcpClient = tcpClient;
        this.stream = tcpClient.GetStream();
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];
        sendBuffer = new byte[Const.SEND_BUFFER_SIZE];

        Start();
    }

    int currentBufferIndex = 0;
    void Start() {
        ReceivePacket(Const.PACKET_LENGTH_HEADER_SIZE);

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
            ProtocolHandler.GetInstance().ProtocolHandle(protocol, this);
        }

        stream.Flush();


        Start();
    }

    async void ReceivePacket(int packet_size) {
        int bytesReceived = await stream.ReadAsync(receiveBuffer, 0, packet_size);

        while (bytesReceived >= packet_size) {
            bytesReceived += await stream.ReadAsync(receiveBuffer, 0, packet_size - bytesReceived);
        }
    }

    public void SendPacket(IProtocol protocol) {
        using (BinaryWriter bw = new BinaryWriter(stream)) {
            protocol.Write(bw);
        }

        stream.WriteAsync(sendBuffer);
    }
}