using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

class Program {
    public static TcpHandler sessionHandler = null;
    public static TcpHandler battleHandler = null;

    static void StartGame() {
        TcpClient tcpClient = new TcpClient("127.0.0.1", Const.SESSION_SERVER_PORT);

        Console.WriteLine("Session Server Connected!");

        Task.Run(() => {
            var sw = new Stopwatch();
            int frameLimit = 1000 / 60; // 1000ms / 60fps

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

                sw.Stop();
                var elapsed = (int)sw.ElapsedMilliseconds;
                var sleepDuration = frameLimit - elapsed;
                if (sleepDuration > 0) {
                    Thread.Sleep(sleepDuration);
                }
            }
        });

        sessionHandler = new TcpHandler(tcpClient, 0);

        sessionHandler.SendPacket(new Login_REQ_C2S {
            PID = DateTime.Now.Ticks.ToString(),
        });

        Console.WriteLine("Send : [Login_REQ_C2S]");
    }

    static void Main(String[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolDispatcher.GetInstance().Register();
        StartGame();
        bool gameRunning = true;
        while (gameRunning) {

        }
    }
}