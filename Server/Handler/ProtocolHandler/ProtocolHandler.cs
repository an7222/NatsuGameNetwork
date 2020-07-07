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
    Dictionary<int, Action<IProtocol, TcpSessionHandler>> HandlePool = new Dictionary<int, Action<IProtocol, TcpSessionHandler>>();

    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            Action<IProtocol, TcpSessionHandler> action = createAction_session(instance);
            if(action == null) {
                action = createAction_battle(instance);
            }
            HandlePool.Add(instance.GetProtocol_ID(), action);
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

    bool IsBattleHandler(TcpSessionHandler handler) {
        return handler is TcpSessionHandler_Battle;
    }
}

