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
    Dictionary<IProtocol, Action<TcpClientHandler>> HandlePool = new Dictionary<IProtocol, Action<TcpClientHandler>>();
    
    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            HandlePool.Add(instance, createAction(instance));
        }
    }

    public void ProtocolHandle(IProtocol protocol, TcpClientHandler handler) {
        Action<TcpClientHandler> action;
        if (HandlePool.TryGetValue(protocol, out action)) {
            action(handler);
        }
    }

    Action<TcpClientHandler> createAction(IProtocol protocol) {
        Action<TcpClientHandler> action = null;
                    
        if(protocol is Login) {
            action = (TcpClientHandler handler) => {
                Console.WriteLine("Is Login Protocol!");
                SessionServer.GetInstance().SendPacketAll(protocol);
            };
        }

        return action;
    }
}

