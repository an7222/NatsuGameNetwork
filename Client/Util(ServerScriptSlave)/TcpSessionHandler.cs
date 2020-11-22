using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Channels;

class TcpSessionHandler {
    byte[] receiveBuffer;
    NetworkStream networkStream = null;
    public int channel_id = 0;

    public TcpSessionHandler(TcpClient tcpClient, bool isSessionServer, int channel_id) {
        this.networkStream = tcpClient.GetStream();
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];

        this.channel_id = channel_id;
        ProcessReceive();
    }

    async void ProcessReceive() {
        int bytesReceived = 0;
        while (true) {
            try {
                while (bytesReceived < Const.PACKET_LENGTH_HEADER_SIZE) {
                    bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, receiveBuffer.Length - bytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return;
            }

            int packetLength = BitConverter.ToInt32(receiveBuffer);

            if (packetLength <= 0) {
                Console.WriteLine("No Packet");
                continue;
            }

            try {
                while (bytesReceived < packetLength) {
                    bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, receiveBuffer.Length - bytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return;
            }

            int bodyLength = packetLength - Const.PACKET_LENGTH_HEADER_SIZE;
            byte[] optimizeBuffer = new byte[bodyLength];
            Array.Copy(receiveBuffer, Const.PACKET_LENGTH_HEADER_SIZE, optimizeBuffer, 0, bodyLength);

            using (var ms = new MemoryStream(optimizeBuffer))
            using (var br = new BinaryReader(ms)) {
                int protocol_id = br.ReadInt32();

                var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                if (protocol != null) {
                    protocol.Read(br);
                    ProtocolHandler.GetInstance().Protocol_Logic(protocol, this);
                }
            }

            int nextBufferSize = receiveBuffer.Length - packetLength;
            Array.Copy(receiveBuffer, packetLength, receiveBuffer, 0, nextBufferSize);
            Array.Clear(receiveBuffer, nextBufferSize, receiveBuffer.Length - nextBufferSize);

            bytesReceived -= packetLength;
        }
    }

    public void SendPacket(IProtocol protocol) {
        using (var bw = new BinaryWriter(networkStream, Encoding.Default, true)) {
            protocol.Write(bw);
        }
    }
}