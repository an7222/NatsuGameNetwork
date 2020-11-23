using System;
using System.Collections.Generic;
using System.Text;

class RunServerManager {
}

interface IRealTimeServer : IRunServer {
    public void AddClient(TcpHandler handler);
    public void RemoveClient(int session_id);
}

interface IRunServer {
    public void Start();
}