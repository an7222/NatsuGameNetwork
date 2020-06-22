using System;
class Program {
    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        SessionServer.GetInstance().Start();
        BattleServer.GetInstance().Start();
        RestAPIServer.GetInstance().Start();

        //ThreadManager.GetInstance().RegisterWork(() => {
        //    Console.WriteLine("Test");
        //});

        while (true) {
            ThreadManager.GetInstance().RegisterWork(() => {
                foreach (var con in BattleServer.GetInstance().GetFieldControllerPool()) {
                    con.Update();
                }
            });
        }
    }
}
