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
    Dictionary<int, Action<IProtocol, TcpSessionHandler>> HandlePool = new Dictionary<int, Action<IProtocol, TcpSessionHandler>>();
    
    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            HandlePool.Add(instance.GetProtocol_ID(), createAction(instance));
        }
    }

    public void Protocol_Logic(IProtocol protocol, TcpSessionHandler handler) {
        Action<IProtocol, TcpSessionHandler> action;
        if (HandlePool.TryGetValue(protocol.GetProtocol_ID(), out action)) {
            action(protocol, handler);
        } else {
            Console.WriteLine("No Protocol ID");
        }
    }

    Action<IProtocol, TcpSessionHandler> createAction(IProtocol dummyProtocol) {
        Action<IProtocol, TcpSessionHandler> action = null;
                    
        if(dummyProtocol is Login_RES_S2C) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as Login_RES_S2C;
                Console.WriteLine("Receive : [Login_RES_S2C]");

                handler.SendPacket(new Login_FIN_C2S());

                TcpClient tcpClient = new TcpClient("127.0.0.1", Const.BATTLE_SERVER_PORT);

                Program.battleHandler = new TcpSessionHandler(tcpClient, false, cast.CHANNEL_ID);

                Program.battleHandler.SendPacket(new NewBattleUser_REQ_C2B {
                    USER_ID = 1,
                });

                Console.WriteLine("Send : [NewBattleUser_REQ_C2B]");

                Console.WriteLine("Send : [Login_FIN_C2S]");
            };
        }
        if (dummyProtocol is NewBattleUser_RES_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as NewBattleUser_RES_C2B;
                Console.WriteLine("Receive : [NewBattleUser_RES_C2B]");

                Program.battleHandler.SendPacket(new NewBattleUser_FIN_C2B {
                    CHANNEL_ID = Program.battleHandler.channel_id,
                });

                Console.WriteLine("Battle Server Connected!");
            };
        } else if (dummyProtocol is MoveStart_B2C) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveStart_B2C;
                Console.WriteLine("Receive : [MoveStart_B2C]");
            };
        } else if (dummyProtocol is MoveEnd_B2C) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveEnd_B2C;
                Console.WriteLine("Receive : [MoveEnd_B2C]");
            };
        } else if (dummyProtocol is ChangePos_B2C) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as ChangePos_B2C;
                Console.WriteLine("Receive : [ChangePos_B2C]");
            };
        }

        return action;
    }
}

