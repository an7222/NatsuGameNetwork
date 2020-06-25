using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

class ProtocolHandler : Singleton<ProtocolHandler> {
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

        if (dummyProtocol is Login_REQ_C2S) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as Login_REQ_C2S;
                Console.WriteLine("Receive : [Login_REQ_C2S]");
                Random r = new Random();
                handler.SendPacket(new Login_RES_S2C {
                    USER_ID = SessionServer.GetInstance().GetUniqueUserID(),
                    ServerTimeUnix = DateTime.Now.Ticks,
                    SessionToken = Guid.NewGuid().ToString(),
                    FIELD_ID = r.Next(1, 2), //TODO read for RESTAPI
                });

                Console.WriteLine("Send : [Login_RES_S2C]");
            };
        } else if (dummyProtocol is Login_FIN_C2S) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                Console.WriteLine("Receive : [Login_FIN_C2S]");
                SessionServer.GetInstance().AddClient(handler);
            };
        } else if (dummyProtocol is NewBattleUser_REQ_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as NewBattleUser_REQ_C2B;
                Console.WriteLine("Receive : [NewBattleUser_REQ_C2B]");

                if(false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;

                Console.WriteLine("FIELD ID : " + cast.FIELD_ID);
                battleHandler.FIELD_ID = cast.FIELD_ID;
                BattleServer.GetInstance().AddClient(battleHandler);
                battleHandler.SendPacket(new NewBattleUser_RES_C2B {
                    ObjectIDList = 4,
                    TODOStatusList = 5,
                });

                Console.WriteLine("Send : [NewBattleUser_RES_C2B]");
            };
        } else if (dummyProtocol is MoveStart_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveStart_C2B;
                Console.WriteLine("Receive : [MoveStart_C2B]");

                if (false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;

                BattleServer.GetInstance().SendPacketField(new MoveStart_B2C {
                    OBJECT_ID = 1,
                    Direction = cast.Direction,
                }, battleHandler.FIELD_ID);

                Console.WriteLine("SendField : [MoveStart_B2C]");
            };
        } else if (dummyProtocol is MoveEnd_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveEnd_C2B;
                Console.WriteLine("Receive : [MoveEnd_C2B]");

                if (false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;

                BattleServer.GetInstance().SendPacketField(new MoveEnd_B2C {
                }, battleHandler.FIELD_ID);

                Console.WriteLine("SendField : [MoveEnd_B2C]");
            };
        }
        return action;
    }

    bool IsBattleHandler(TcpSessionHandler handler) {
        return handler is TcpSessionHandler_Battle;
    }
}

