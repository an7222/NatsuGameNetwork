using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program {
    const int SESSION_SERVER_PORT = 8001;
    const int RECEIVE_BUFFER_SIZE = 256;
    const int PACKET_LENGTH_HEADER_SIZE = 4;
    static byte[] receiveBuffer;
    static NetworkStream networkStream;

    async static void StartClient() {
        TcpClient tcpClient = new TcpClient("127.0.0.1", SESSION_SERVER_PORT);

        Console.WriteLine("Session Server Connected!");

        Task consoleReadTask = new Task(() => {
            while (true) {
                var chat = Console.ReadKey();
                if (chat.Key == ConsoleKey.UpArrow) {
                    Send(new Login {
                        PID = "antori",
                        LoginAt = 1234,
                    });
                }
            }
        });
        consoleReadTask.Start();

        networkStream = tcpClient.GetStream();
        receiveBuffer = new byte[RECEIVE_BUFFER_SIZE];

        while (true) {
            int bytesReceived = await networkStream.ReadAsync(receiveBuffer, 0, RECEIVE_BUFFER_SIZE).ConfigureAwait(false);

            int packetLength = BitConverter.ToInt32(receiveBuffer);
            Console.WriteLine("packet Length : " + packetLength);

            if (packetLength <= 0) {
                Console.WriteLine("No Packet");
                continue;
            }

            while (bytesReceived <= packetLength) {
                bytesReceived += await networkStream.ReadAsync(receiveBuffer, bytesReceived, receiveBuffer.Length - bytesReceived).ConfigureAwait(false);
            }

            int bodyLength = packetLength - PACKET_LENGTH_HEADER_SIZE;
            byte[] optimizeBuffer = new byte[bodyLength];
            Array.Copy(receiveBuffer, PACKET_LENGTH_HEADER_SIZE, optimizeBuffer, 0, bodyLength);

            using (MemoryStream ms = new MemoryStream(optimizeBuffer))
            using (BinaryReader br = new BinaryReader(ms)) {
                int protocol_id = br.ReadInt32();
                Console.WriteLine("Protocol ID : " + protocol_id);

                var protocol = ProtocolManager.GetInstance().GetProtocol(protocol_id);
                if (protocol != null) {
                    protocol.Read(br);
                }
            }


            Array.Clear(receiveBuffer, 0, receiveBuffer.Length);
        }
    }

    private static void Send(IProtocol protocol) {
        using (BinaryWriter bw = new BinaryWriter(networkStream, Encoding.Default, true)) {
            protocol.Write(bw);

            byte[] writeBuffer = new byte[protocol.GetPacketLength()];
            networkStream.Write(writeBuffer);
        }
    }

    static void Main(String[] args) {
        ProtocolManager.GetInstance().Register();
        StartClient();
        bool gameRunning = true;
        while (gameRunning) {

        }
    }
}