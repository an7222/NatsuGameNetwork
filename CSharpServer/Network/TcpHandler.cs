using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

class TcpHandler : TickBase {
    byte[] receiveBuffer;
    NetworkStream networkStream = null;
    int SESSION_ID;
    IRealTimeServer connectedServer;

    public TcpHandler(TcpClient tcpClient, int session_id, IRealTimeServer connectedServer) {
        this.networkStream = tcpClient.GetStream();
        this.SESSION_ID = session_id;
        this.connectedServer = connectedServer;
        receiveBuffer = new byte[Const.RECEIVE_BUFFER_SIZE];

        ProcessReceive();
        ProcessSend();
    }

    async void ProcessReceive() {
        while (true) {
            int headerBytesReceived = 0;
            try {
                while (headerBytesReceived < Const.PACKET_HEADER_LENGTH) {
                    headerBytesReceived += await networkStream.ReadAsync(receiveBuffer, headerBytesReceived, Const.PACKET_HEADER_LENGTH - headerBytesReceived).ConfigureAwait(false);
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

            bool bodyOverflow = false;
            int bodyLength = packetLength - Const.PACKET_HEADER_LENGTH;
            if (bodyLength > Const.RECEIVE_BUFFER_SIZE) {
                Array.Resize(ref receiveBuffer, bodyLength);
                bodyOverflow = true;
            }

            int bodyBytesReceived = 0;
            try {
                while (bodyBytesReceived < bodyLength) {
                    bodyBytesReceived += await networkStream.ReadAsync(receiveBuffer, bodyBytesReceived, bodyLength - bodyBytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                connectedServer.RemoveClient(SESSION_ID);
                return;
            }

            using (var ms = new MemoryStream(receiveBuffer))
            using (var br = new BinaryReader(ms)) {
                int protocol_id = br.ReadInt32();

                var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                if (protocol != null) {
                    protocol.Read(br);
                    ProtocolDispatcher.GetInstance().Dispatch(protocol, this);
                }
            }

            if (bodyOverflow) {
                Array.Resize(ref receiveBuffer, Const.RECEIVE_BUFFER_SIZE);
            }
        }
    }

    AutoResetEvent autoEvent = new AutoResetEvent(false);
    void ProcessSend() {
        Task sendTask = new Task(() => {
            while (true) {
                Update();
                autoEvent.WaitOne();
            }
        });

        sendTask.Start();
    }

    public void SendPacket(IProtocol protocol) {
        EnqueueAction(() => {
            using (var bw = new BinaryWriter(networkStream, Encoding.Default, true)) {
                try {
                    protocol.Write(bw);
                } catch (Exception e) {
                    Console.WriteLine(e);
                    connectedServer.RemoveClient(SESSION_ID);
                }
            }
        });

        autoEvent.Set();
    }
}