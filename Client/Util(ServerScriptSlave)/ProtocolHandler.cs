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
                    
        if(dummyProtocol is Login_RES_S2C) {
            action = (IProtocol protocol, TcpClientHandler handler) => {
                var temp = protocol as Login_RES_S2C;
                Console.WriteLine("Receive! [Login_ACK_S2C]\nUserID : {0}, ServerTimeUnix : {1}, SessionToken : {2}", temp.UserID, temp.ServerTimeUnix, temp.SessionToken);
                handler.SendPacket(new Login_FIN_C2S());

                TcpClient tcpClient = new TcpClient(temp.BattleServerIp, Const.BATTLE_SERVER_PORT);

                Console.WriteLine("Battle Server Connected!");

                Program.battleHandler = new TcpClientHandler(tcpClient, false);
            };
        } else if (dummyProtocol is MoveStart_B2C) {
            action = (IProtocol protocol, TcpClientHandler handler) => {
                var temp = protocol as MoveStart_B2C;
                Console.WriteLine("Receive! [MoveStart_B2C]\nObjectID : {0}, Direction : {1}", temp.ObjectID, temp.Direction);
            };
        }

        return action;
    }
}

