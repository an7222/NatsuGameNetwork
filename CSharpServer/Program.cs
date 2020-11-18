class Program {
    static void Main(string[] args) {
        ProtocolManager.GetInstance().Register();
        ProtocolHandler.GetInstance().Register();
        SessionServer.GetInstance().Start();
        BattleServer.GetInstance().Start();

        while (true) {
            foreach (var channelController in BattleServer.GetInstance().GetChannelControllerPool()) {
                //TODO : Frame Sync
                channelController.Update();
            }
        }
    }
}