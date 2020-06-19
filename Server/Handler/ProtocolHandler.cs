using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

class ProtocolHandler : Singleton<ProtocolHandler>{
    Dictionary<int, Action<IProtocol, TcpClientHandler>> HandlePool = new Dictionary<int, Action<IProtocol, TcpClientHandler>>();
    
    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            HandlePool.Add(instance.GetProtocol_ID(), createAction(instance));
        }
    }

    public void Protocol_Logic(IProtocol protocol, TcpClientHandler handler) {
        Action<IProtocol, TcpClientHandler> action;
        if (HandlePool.TryGetValue(protocol.GetProtocol_ID(), out action)) {
            action(protocol, handler);
        } else {
            Console.WriteLine("No Protocol ID");
        }
    }

    Action<IProtocol, TcpClientHandler> createAction(IProtocol dummyProtocol) {
        Action<IProtocol, TcpClientHandler> action = null;
                    
        if(dummyProtocol is Login_REQ_C2S) {
            action = (IProtocol protocol, TcpClientHandler handler) => {
                var temp = protocol as Login_REQ_C2S;
                Console.WriteLine("Receive! [Login_C2S]\nPID : {0}", temp.PID);
                handler.SendPacket(new Login_RES_S2C {
                    UserID = SessionServer.GetInstance().GetUserID(),
                    ServerTimeUnix = DateTime.Now.Ticks,
                    SessionToken = Guid.NewGuid().ToString(),
                    BattleServerIp = "127.0.0.1",
                });
            };
        } else if (dummyProtocol is Login_FIN_C2S) {
            action = (IProtocol protocol, TcpClientHandler handler) => {
                Console.WriteLine("Receive! [Login_FIN_C2S]\n");
                SessionServer.GetInstance().AddClient(handler);
            };
        } else if (dummyProtocol is MoveStart_C2B) {
            action = (IProtocol protocol, TcpClientHandler handler) => {
                var temp = protocol as MoveStart_C2B;
                Console.WriteLine("Receive! [MoveStart_C2B]\n Direction : {0}", temp.Direction);

                BattleServer.GetInstance().SendPacketAll(new MoveStart_B2C {
                    ObjectID = 1,
                    Direction = temp.Direction,
                });
            };
        }

        return action;
    }
}

