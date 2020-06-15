using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

class ProtocolHandler : Singleton<ProtocolHandler>{
    Dictionary<IProtocol, Action> HandlePool = new Dictionary<IProtocol, Action>();
    
    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            HandlePool.Add(instance, createAction(instance));
        }
    }

    public void ProtocolHandle(IProtocol protocol) {
        Action action;
        if (HandlePool.TryGetValue(protocol, out action)) {
            action();
        }
    }

    Action createAction(IProtocol protocol) {
        Action action = null;
                    
        if(protocol is Login) {
            action = () => {
                Console.WriteLine("Is Login Protocol!");
            };
        }

        return action;
    }
}

