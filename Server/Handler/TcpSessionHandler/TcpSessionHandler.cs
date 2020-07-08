﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

class TcpSessionHandler {
    byte[] receiveBuffer;
    TcpClient tcpClient = null;
    NetworkStream networkStream = null;
    int SESSION_ID;
    IRealTimeServer connectedServer;

    public TcpSessionHandler(TcpClient tcpClient, int session_id, IRealTimeServer connectedServer) {
        this.tcpClient = tcpClient;
        this.networkStream = tcpClient.GetStream();
        this.SESSION_ID = session_id;
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
                connectedServer.RemoveClient(SESSION_ID);
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
                connectedServer.RemoveClient(SESSION_ID);
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
            connectedServer.RemoveClient(SESSION_ID);
            return;
        }
    }

    //TODO : need flush?
    void flush() {
        networkStream.Flush();
    }
}