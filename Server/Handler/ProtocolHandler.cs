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
        }
    }

    Action<IProtocol, TcpClientHandler> createAction(IProtocol dummyProtocol) {
        Action<IProtocol, TcpClientHandler> action = null;
                    
        if(dummyProtocol is Login) {
            action = (IProtocol protocol, TcpClientHandler handler) => {
                Console.WriteLine("Is Login Protocol!");
                var temp = protocol as Login;
                Console.WriteLine("{0}, {1}, {2}, {3}", temp.PACKET_LENGTH, temp.PROTOCOL_ID, temp.PID, temp.LoginAt);
                //SessionServer.GetInstance().SendPacketAll(protocol);
            };
        }

        return action;
    }
}

