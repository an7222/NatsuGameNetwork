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
    static NetworkStream networkStream;

    static void StartClient() {
        TcpClient tcpClient = new TcpClient("127.0.0.1", SESSION_SERVER_PORT);

        Console.WriteLine("Session Server Connected!");

        Task consoleReadTask = new Task(() => {
            while (true) {
                var chat = Console.ReadKey();
                if (chat.Key == ConsoleKey.UpArrow) {
                }
            }
        });
        consoleReadTask.Start();

        TcpClientHandler handler = new TcpClientHandler(tcpClient);
    }

    static void Main(String[] args) {
        ProtocolManager.GetInstance().Register();
        StartClient();
        bool gameRunning = true;
        while (gameRunning) {

        }
    }
}