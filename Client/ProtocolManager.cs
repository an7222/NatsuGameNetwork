using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

interface IProtocol {
    public int GET_PROTOCOL_ID();
}

class ProtocolManager : Singleton<ProtocolManager> {
    Dictionary<int, IProtocol> protocolPool = new Dictionary<int, IProtocol>();
    public Dictionary<int, IProtocol> Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            protocolPool.Add(instance.GET_PROTOCOL_ID(), instance);
        }

        return protocolPool;
    }

    public Dictionary<int, IProtocol> GetProtocolPool() {
        return protocolPool;
    }
}
