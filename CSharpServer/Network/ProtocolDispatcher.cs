using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

partial class ProtocolDispatcher : Singleton<ProtocolDispatcher> {
    Dictionary<int, Action<IProtocol, TcpHandler>> HandlePool = new Dictionary<int, Action<IProtocol, TcpHandler>>();

    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            Action<IProtocol, TcpHandler> action = createAction_session(instance);
            action ??= createAction_battle(instance);
            HandlePool.Add(instance.GetProtocol_ID(), action);
        }
    }

    public void Dispatch(IProtocol protocol, TcpHandler handler) {
        if (HandlePool.TryGetValue(protocol.GetProtocol_ID(), out var action)) {
            action(protocol, handler);
        } else {
            Console.WriteLine("No Protocol ID");
        }
    }

    bool IsBattleHandler(TcpHandler handler) {
        return handler is TcpHandler_Battle;
    }
}

