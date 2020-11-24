using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

class TcpHandler : TickBase {
    byte[] headerBuffer;
    byte[] bodyBuffer;
    NetworkStream networkStream = null;
    public int channel_id = 0;

    public TcpHandler(TcpClient tcpClient, int channel_id) {
        this.networkStream = tcpClient.GetStream();
        headerBuffer = new byte[Const.HEADER_BUFFER_SIZE];
        bodyBuffer = new byte[Const.BODY_BUFFER_SIZE];

        this.channel_id = channel_id;
        ProcessReceive();
        ProcessSend();
    }

    async void ProcessReceive() {
        while (true) {
            int headerBytesReceived = 0;
            try {
                while (headerBytesReceived < Const.HEADER_BUFFER_SIZE) {
                    headerBytesReceived += await networkStream.ReadAsync(headerBuffer, headerBytesReceived, headerBuffer.Length - headerBytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return;
            }


            int packetLength = BitConverter.ToInt32(headerBuffer);

            if (packetLength <= 0) {
                Console.WriteLine("No Packet");
                continue;
            }

            bool bodyOverflow = false;
            int bodyLength = packetLength - Const.HEADER_BUFFER_SIZE;
            if (bodyLength > Const.BODY_BUFFER_SIZE) {
                Array.Resize(ref bodyBuffer, bodyLength);
                bodyOverflow = true;
            }
            int dataBytesReceived = 0;
            try {
                while (dataBytesReceived < bodyLength) {
                    dataBytesReceived += await networkStream.ReadAsync(bodyBuffer, dataBytesReceived, bodyLength - dataBytesReceived).ConfigureAwait(false);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
                return;
            }

            using (var ms = new MemoryStream(bodyBuffer))
            using (var br = new BinaryReader(ms)) {
                int protocol_id = br.ReadInt32();

                var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                if (protocol != null) {
                    protocol.Read(br);
                    ProtocolDispatcher.GetInstance().Dispatch(protocol, this);
                }
            }

            if (bodyOverflow) {
                Array.Resize(ref bodyBuffer, Const.BODY_BUFFER_SIZE);
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
                    return;
                }
            }
        });

        autoEvent.Set();
    }
}