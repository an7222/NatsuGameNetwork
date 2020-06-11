using System;
class Program {
    static void Main(string[] args) {
        SessionServer.GetInstance().Start();

        bool gameLoop = false;
        //ThreadManager.GetInstance().RegisterWork(() => {
        //    Console.WriteLine("Test");
        //});

        while (!gameLoop) {

        }
    }
}
