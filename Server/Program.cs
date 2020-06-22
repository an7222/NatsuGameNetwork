using System;
class Program {
    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        SessionServer.GetInstance().Start();
        BattleServer.GetInstance().Start();
        RestAPIServer.GetInstance().Start();


        while (true) {
            foreach (var con in BattleServer.GetInstance().GetFieldControllerPool()) {
                ThreadManager.GetInstance().RegisterWork(() => {
                    con.Update();
                });
            }
        }
    }
}
