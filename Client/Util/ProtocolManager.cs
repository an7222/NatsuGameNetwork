using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

interface IProtocol {
    public void SetPacketLength();
    public int GetPacketLength();
    public int GetProtocol_ID();

    public void Read(BinaryReader br);
    public void Write(BinaryWriter bw);
}

class ProtocolManager : Singleton<ProtocolManager> {
    Dictionary<int, IProtocol> protocolPool = new Dictionary<int, IProtocol>();
    public void Register() {
        var baseType = typeof(IProtocol);
        var a = Assembly.GetAssembly(baseType).GetTypes().Where(t => baseType != t && baseType.IsAssignableFrom(t));
        foreach (var b in a) {
            IProtocol instance = (IProtocol)Activator.CreateInstance(b);
            protocolPool.Add(instance.GetProtocol_ID(), instance);
        }
    }

    public IProtocol GetProtocol(int protocol_id) {
        IProtocol ret;
        if (false == protocolPool.TryGetValue(protocol_id, out ret)) {
            Console.WriteLine("No Protocol");
        }

        ret = (IProtocol)Activator.CreateInstance(ret.GetType());

        return ret;
    }
}
