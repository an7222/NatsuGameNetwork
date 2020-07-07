using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

partial class ProtocolHandler : Singleton<ProtocolHandler> {
    Action<IProtocol, TcpSessionHandler> createAction_session(IProtocol dummyProtocol) {
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
        }
        return action;
    }
}

