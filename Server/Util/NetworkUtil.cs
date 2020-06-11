using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

class NetworkUtil {
    public static void OnSocketNotFindException(Socket socket, Exception e, Dictionary<int, Socket> connectedSocketPool) {
        Console.WriteLine(e);
        socket.Close();
        int closeSocketId = connectedSocketPool.Where(w => w.Value == socket).Select(s => s.Key).FirstOrDefault();
        connectedSocketPool.Remove(closeSocketId);
    }
}
