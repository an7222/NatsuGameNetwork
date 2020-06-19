using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO;

class Program {
    public static TcpClientHandler sessionHandler = null;
    public static TcpClientHandler battleHandler = null;

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
                } else if (input.Key == ConsoleKey.DownArrow) {
                    battleHandler.SendPacket(new MoveStart_C2B {
                        Direction = (int)Direction.Down,
                    });
                } else if (input.Key == ConsoleKey.Spacebar) {
                    battleHandler.SendPacket(new MoveEnd_C2B {
                    });
                }
            }
        });
        consoleReadTask.Start();

        sessionHandler = new TcpClientHandler(tcpClient, true);
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