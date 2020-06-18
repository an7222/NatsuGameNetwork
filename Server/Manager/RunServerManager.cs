using System;
using System.Collections.Generic;
using System.Text;

class RunServerManager {
}

interface IRealTimeServer : IRunServer {
    public void SendPacketAll(IProtocol protocol);
    public void OnClientLeave(int session_id);
}

interface IRunServer {
    public void Start();
}