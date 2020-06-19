using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class TcpSessionHandler {
    byte[] receiveBuffer;
    TcpClient tcpClient = null;
    NetworkStream networkStream = null;

    public TcpSessionHandler(TcpClient tcpClient, bool isSessionServer, int field_id) {
        this.tcpClient = tcpClient;
        this.networkStream = tcpClient.GetStream();
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];

        if (isSessionServer) {
            SendPacket(new Login_REQ_C2S {
                PID = DateTime.Now.Ticks.ToString(),
            });

            Console.WriteLine("Send : [Login_REQ_C2S]");
        } else {
            SendPacket(new NewBattleUser_REQ_C2B {
                UserID = 1,
                FieldId = field_id,
            });
        }

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
                return;
            }


            int packetLength = BitConverter.ToInt32(receiveBuffer);

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


            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
        }
    }

    public void SendPacket(IProtocol protocol) {
        using (var bw = new BinaryWriter(networkStream, Encoding.Default, true)) {
            protocol.Write(bw);
        }

        
        byte[] writeBuffer = new byte[protocol.GetPacketLength()];

        try {
            networkStream.Write(writeBuffer);
        } catch (Exception e) {
            Console.WriteLine(e);
            return;
        }
    }

    //TODO : need flush?
    void flush() {
        networkStream.Flush();
    }
}