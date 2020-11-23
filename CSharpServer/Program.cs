using System;
using System.Diagnostics;
using System.Threading;

class Program {

    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        SessionServer.GetInstance().Start();
        BattleServer.GetInstance().Start();

        var sw = new Stopwatch();
        int frameLimit = 1000 / 120; // 1000ms / 120fps
        while (true) {
            sw.Restart();

            foreach (var channelController in BattleServer.GetInstance().GetChannelControllerPool()) {
                ThreadManager.GetInstance().RegisterWork(() => {
                    channelController.Update();
                });
            }

            sw.Stop();
            var elapsed = (int)sw.ElapsedMilliseconds;
            var sleepDuration = frameLimit - elapsed;
            if (sleepDuration > 0) {
                Thread.Sleep(sleepDuration);
            }
        }
    }
}