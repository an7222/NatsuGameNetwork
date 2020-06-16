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
        while (true) {
            {
                int bytesReceived = await networkStream.ReadAsync(receiveBuffer, 0, Const.PACKET_LENGTH_HEADER_SIZE).ConfigureAwait(false);

                Console.WriteLine("{0}, {1}", bytesReceived, Const.PACKET_LENGTH_HEADER_SIZE);
                while (bytesReceived > Const.PACKET_LENGTH_HEADER_SIZE) {
                    bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, Const.PACKET_LENGTH_HEADER_SIZE - bytesReceived).ConfigureAwait(false);
                }
            }

            int packetLength = BitConverter.ToInt32(receiveBuffer);
            Console.WriteLine("packet Length : " + packetLength);

            {
                int bytesReceived = await networkStream.ReadAsync(receiveBuffer, 0, packetLength).ConfigureAwait(false);

                while (bytesReceived > packetLength) {
                    bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, packetLength - bytesReceived).ConfigureAwait(false);
                }
            }

            byte[] optimizeBuffer = new byte[packetLength];
            Array.Copy(receiveBuffer, optimizeBuffer, packetLength);
            using (MemoryStream ms = new MemoryStream(optimizeBuffer)) {
                using (BinaryReader br = new BinaryReader(ms)) {
                    int protocol_id = br.ReadInt32();
                    Console.WriteLine("Protocol ID : " + protocol_id);

                    var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                    if(protocol != null) {
                        protocol.Read(br);
                        ProtocolHandler.GetInstance().ProtocolAction(protocol, this);
                    }
                }
            }

            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
        }
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