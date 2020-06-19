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
                    UserID = SessionServer.GetInstance().GetUserID(),
                    ServerTimeUnix = DateTime.Now.Ticks,
                    SessionToken = Guid.NewGuid().ToString(),
                    FieldId = r.Next(1, 2), //TODO read for RESTAPI
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

                Console.WriteLine("FIELD ID : " + cast.FieldId);
                handler.SetFieldId(cast.FieldId);
                BattleServer.GetInstance().AddFieldCLient(handler, cast.FieldId);
                BattleServer.GetInstance().AddClient(handler);
                handler.SendPacket(new NewBattleUser_RES_C2B {
                    ObjectIDList = 4,
                    TODOStatusList = 5,
                });

                Console.WriteLine("Send : [NewBattleUser_RES_C2B]");
            };
        } else if (dummyProtocol is MoveStart_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveStart_C2B;
                Console.WriteLine("Receive : [MoveStart_C2B]");

                BattleServer.GetInstance().SendPacketField(new MoveStart_B2C {
                    ObjectID = 1,
                    Direction = cast.Direction,
                }, handler.GetFieldId());

                Console.WriteLine("SendField : [MoveStart_B2C]");
            };
        } else if (dummyProtocol is MoveEnd_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveEnd_C2B;
                Console.WriteLine("Receive : [MoveEnd_C2B]");

                BattleServer.GetInstance().SendPacketField(new MoveEnd_B2C {
                }, handler.GetFieldId());

                Console.WriteLine("SendField : [MoveEnd_B2C]");
            };
        }
        return action;
    }
}

