using System;
class Program {
    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        SessionServer.GetInstance().Start();

        bool gameLoop = false;
        //ThreadManager.GetInstance().RegisterWork(() => {
        //    Console.WriteLine("Test");
        //});

        while (!gameLoop) {

        }
    }
}
