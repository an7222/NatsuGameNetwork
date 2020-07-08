using System;
using System.Threading;

class Program {
    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        SessionServer.GetInstance().Start();
        BattleServer.GetInstance().Start();
        RestAPIServer.GetInstance().Start();


        while (true) {
            foreach (var channelController in BattleServer.GetInstance().GetChannelControllerPool()) {
                channelController.Update();
            }
        }
    }
}
