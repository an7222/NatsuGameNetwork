interface IRealTimeServer : IRunServer {
    public void AddClient(TcpHandler handler);
    public void RemoveClient(int session_id);
}

interface IRunServer {
    public void Start();
}