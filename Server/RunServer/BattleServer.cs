using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

class BattleServer : Singleton<BattleServer>, IRealTimeServer {
    int session_id = 1;
    Dictionary<int, TcpSessionHandler> connectedClientPool = new Dictionary<int, TcpSessionHandler>();
    List<Field> fieldList = new List<Field>();
    Dictionary<int, List<TcpSessionHandler>> fieldClientPool = new Dictionary<int, List<TcpSessionHandler>>();

    public void Start() {
        //TODO : Read for DB or Excel
        fieldList.Add(new Field {
            FieldId = 1,
            FieldName = "안토리네 집",
        });
        fieldList.Add(new Field {
            FieldId = 2,
            FieldName = "깃허브",
        });

        for (int i = 0; i < fieldList.Count; ++i) {
            ThreadManager.GetInstance().RegisterWork(() => {
                while (true) {

                }
            });
        }

        TcpListener listener = new TcpListener(IPAddress.Any, Const.BATTLE_SERVER_PORT);

        try {
            listener.Start();
            listener.BeginAcceptTcpClient(OnAccept, listener);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("Battle Server Start");
    }

    void OnAccept(IAsyncResult ar) {
        Console.WriteLine("Client Connected");
        TcpListener listener = (TcpListener)ar.AsyncState;
        TcpClient tcpClient = listener.EndAcceptTcpClient(ar);

        TcpSessionHandler handler = new TcpSessionHandler(tcpClient, session_id, this);

        listener.BeginAcceptTcpClient(OnAccept, listener);
    }

    public void AddClient(TcpSessionHandler handler) {
        if (connectedClientPool.TryAdd(session_id, handler)) {
            session_id = Interlocked.Increment(ref session_id);
        }
    }

    public void RemoveClient(int session_id) {
        TcpSessionHandler handler;
        if (connectedClientPool.TryGetValue(session_id, out handler)) {
            List<TcpSessionHandler> list;
            if(fieldClientPool.TryGetValue(handler.GetFieldId(), out list)){
                list.Remove(handler);
            }
        }
        connectedClientPool.Remove(session_id);
    }

    public void AddFieldCLient(TcpSessionHandler handler, int field_id) {
        List<TcpSessionHandler> list;

        if (false == fieldClientPool.TryGetValue(field_id, out list)) {
            list = new List<TcpSessionHandler>();
            fieldClientPool.Add(field_id, list);
        }

        list.Add(handler);
    }

    public void SendPacketField(IProtocol protocol, int field_id) {
        List<TcpSessionHandler> list;
        Console.WriteLine(field_id);
        if (fieldClientPool.TryGetValue(field_id, out list)) {
            foreach(var handler in list) {
                handler.SendPacket(protocol);
            }
        } else {
            Console.WriteLine("No Field!");
        }
    }
}
