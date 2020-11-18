using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program {
    public static TcpSessionHandler sessionHandler = null;
    public static TcpSessionHandler battleHandler = null;

    static void StartGame() {
        TcpClient tcpClient = new TcpClient("127.0.0.1", Const.SESSION_SERVER_PORT);

        Console.WriteLine("Session Server Connected!");
        Task consoleReadTask = new Task(() => {
            while (true) {
                if (battleHandler == null)
                    continue;

                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.UpArrow) {
                    battleHandler.SendPacket(new MoveStart_C2B {
                        Direction = (int)Direction.Up,
                    });

                    Console.WriteLine("Send : [MoveStart_C2B]");
                } else if (input.Key == ConsoleKey.DownArrow) {
                    battleHandler.SendPacket(new MoveStart_C2B {
                        Direction = (int)Direction.Down,
                    });

                    Console.WriteLine("Send : [MoveStart_C2B]");
                } else if (input.Key == ConsoleKey.Spacebar) {
                    battleHandler.SendPacket(new MoveEnd_C2B {
                    });

                    Console.WriteLine("Send : [MoveEnd_C2B]");
                } else if (input.Key == ConsoleKey.R) {
                    battleHandler.SendPacket(new RestAPI_REQ_C2S {
                    });

                    Console.WriteLine("Send : [RestAPI_REQ_C2S]");
                }
            }
        });
        consoleReadTask.Start();

        sessionHandler = new TcpSessionHandler(tcpClient, true, 0);

        sessionHandler.SendPacket(new Login_REQ_C2S {
            PID = DateTime.Now.Ticks.ToString(),
        });

        Console.WriteLine("Send : [Login_REQ_C2S]");
    }

    static void Main(String[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        StartGame();
        bool gameRunning = true;
        while (gameRunning) {

        }
    }
}