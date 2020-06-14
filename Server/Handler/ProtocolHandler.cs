using Protocol.ClientSession;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

interface IProtocol {
    public int GET_PROTOCOL_ID();
}
class ProtocolHandler {
    public static void Register(Dictionary<int, Action<Stream>> protocolHandlerPool) {
        protocolHandlerPool.Add(Login.PROTOCOL_ID, (Stream a) => {
            var temp = new Login();
            temp.Read(a);
        });
    }
}

