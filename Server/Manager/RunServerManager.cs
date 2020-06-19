using System;
using System.Collections.Generic;
using System.Text;

class RunServerManager {
}

interface IRealTimeServer : IRunServer {
    public void SendPacketAll(IProtocol protocol);
    public void AddClient(TcpSessionHandler handler);
    public void RemoveClient(int session_id);
}

interface IRunServer {
    public void Start();
}