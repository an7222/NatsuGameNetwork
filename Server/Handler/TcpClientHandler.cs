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
    TcpClient tcpClient = null;
    NetworkStream networkStream = null;
    int session_id;
    IRunServer connectedServer;

    public TcpClientHandler(TcpClient tcpClient, int session_id, IRunServer connectedServer) {
        this.tcpClient = tcpClient;
        this.networkStream = tcpClient.GetStream();
        this.session_id = session_id;
        this.connectedServer = connectedServer;
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];
        
        ReceiveProcess();
    }

    async void ReceiveProcess() {
        while (true) {
            int bytesReceived = 0;
            try {
                bytesReceived  = await networkStream.ReadAsync(receiveBuffer, 0, receiveBuffer.Length).ConfigureAwait(false);

                while (bytesReceived <= Const.PACKET_LENGTH_HEADER_SIZE) {
                    bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, receiveBuffer.Length - bytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                connectedServer.OnClientLeave(session_id);
                return;
            }


            int packetLength = BitConverter.ToInt32(receiveBuffer);
            Console.WriteLine("packet Length : " + packetLength);

            if (packetLength <= 0) {
                Console.WriteLine("No Packet");
                continue;
            }

            try {
                while (bytesReceived <= packetLength) {
                    bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, receiveBuffer.Length - bytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                connectedServer.OnClientLeave(session_id);
                return;
            }


            int bodyLength = packetLength - Const.PACKET_LENGTH_HEADER_SIZE;
            byte[] optimizeBuffer = new byte[bodyLength];
            Array.Copy(receiveBuffer, Const.PACKET_LENGTH_HEADER_SIZE, optimizeBuffer, 0, bodyLength);

            using (MemoryStream ms = new MemoryStream(optimizeBuffer))
            using (BinaryReader br = new BinaryReader(ms)) {
                int protocol_id = br.ReadInt32();
                Console.WriteLine("Protocol ID : " + protocol_id);

                var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                if (protocol != null) {
                    protocol.Read(br);
                    ProtocolHandler.GetInstance().Protocol_Logic(protocol, this);
                }
            }


            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
        }
    }

    public void SendPacket(IProtocol protocol) {
        using (BinaryWriter bw = new BinaryWriter(networkStream, Encoding.Default, true)) {
            protocol.Write(bw);
        }

        
        byte[] writeBuffer = new byte[protocol.GetPacketLength()];

        try {
            networkStream.Write(writeBuffer);
        } catch (Exception e) {
            Console.WriteLine(e);
            connectedServer.OnClientLeave(session_id);
            return;
        }
    }

    //TODO : need flush?
    void flush() {
        networkStream.Flush();
    }
}