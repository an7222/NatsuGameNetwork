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

    public TcpClientHandler(TcpClient tcpClient) {
        this.tcpClient = tcpClient;
        this.networkStream = tcpClient.GetStream();
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];
        sendBuffer = new byte[Const.SEND_BUFFER_SIZE];

        ReceiveProcess();
    }

    async void ReceiveProcess() {
        {
            int bytesReceived = await networkStream.ReadAsync(receiveBuffer, 0, Const.PACKET_LENGTH_HEADER_SIZE);

            Console.WriteLine("{0}, {1}", bytesReceived, Const.PACKET_LENGTH_HEADER_SIZE);
            while (bytesReceived > Const.PACKET_LENGTH_HEADER_SIZE) {
                Console.WriteLine("?!");
                bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, Const.PACKET_LENGTH_HEADER_SIZE - bytesReceived);
            }
        }

        int packetLength = 0;
        using (MemoryStream ms = new MemoryStream(receiveBuffer)) {
            using (BinaryReader br = new BinaryReader(ms)) {
                packetLength = br.ReadInt32();
                Console.WriteLine("packet Length : " + packetLength);
            }
        }

        Array.Clear(receiveBuffer, 0, receiveBuffer.Length);


        {
            int bytesReceived = await networkStream.ReadAsync(receiveBuffer, 0, packetLength);

            while (bytesReceived > packetLength) {
                bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, packetLength - bytesReceived);
            }
        }


        using (MemoryStream ms = new MemoryStream(receiveBuffer)) {
            using (BinaryReader br = new BinaryReader(ms)) {
                int protocol_id = br.ReadInt32();
                Console.WriteLine("Protocol ID : " + protocol_id);

                var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                protocol.Read(br);
                ProtocolHandler.GetInstance().ProtocolHandle(protocol, this);
            }
        }

        networkStream.Flush();
        ReceiveProcess();
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