using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class TcpClientHandler {
    byte[] receiveBuffer;
    byte[] sendBuffer;
    TcpClient tcpClient = null;
    NetworkStream networkStream = null;

    private static AutoResetEvent receiveDone =
    new AutoResetEvent(false);

    public TcpClientHandler(TcpClient tcpClient) {
        this.tcpClient = tcpClient;
        this.networkStream = tcpClient.GetStream();
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];
        sendBuffer = new byte[Const.SEND_BUFFER_SIZE];

        ReceiveProcess();
    }

    void ReceiveProcess() {
        using (BinaryReader br = new BinaryReader(networkStream)) {
            receiveDone.Reset();
            ReceivePacket(Const.PACKET_LENGTH_HEADER_SIZE);
            receiveDone.WaitOne();
            int packetLength = br.ReadInt32();
            Console.WriteLine("packet Length : " + packetLength);

            networkStream.Flush();
            receiveDone.Reset();
            ReceivePacket(packetLength);
            receiveDone.WaitOne();
            int protocol_id = br.ReadInt32();
            Console.WriteLine("Protocol ID : " + protocol_id);

            var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
            protocol.Read(br);
            ProtocolHandler.GetInstance().ProtocolHandle(protocol, this);
        }


        ReceiveProcess();
    }

    async void ReceivePacket(int packet_size) {
        int bytesReceived = await networkStream.ReadAsync(receiveBuffer, 0, packet_size);

        while (bytesReceived > packet_size) {
            bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, packet_size - bytesReceived);
        }

        receiveDone.Set();
    }

    public void SendPacket(IProtocol protocol) {
        using (BinaryWriter bw = new BinaryWriter(networkStream)) {
            protocol.Write(bw);
        }

        networkStream.WriteAsync(sendBuffer);

        flush();
    }

    void flush() {
        networkStream.Flush();
    }
}