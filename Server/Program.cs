using System;
class Program {
    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        SessionServer.GetInstance().Start();

        bool gameRunning = true;
        //ThreadManager.GetInstance().RegisterWork(() => {
        //    Console.WriteLine("Test");
        //});

        while (gameRunning) {

        }
    }
}
