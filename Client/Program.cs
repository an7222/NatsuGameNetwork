using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// State object for receiving data from remote device.  
public class StateObject {
    // Client socket.  
    public Socket workSocket = null;
    // Size of receive buffer.  
    public const int BufferSize = 256;
    // Receive buffer.  
    public byte[] buffer = new byte[BufferSize];
    // Received data string.  
    public StringBuilder sb = new StringBuilder();
}

public class AsynchronousClient {
    // The port number for the remote device.  
    private const int SESSION_SERVER_PORT = 8001;

    // ManualResetEvent instances signal completion.  
    private static ManualResetEvent connectDone =
        new ManualResetEvent(false);

    // The response from the remote device.  
    private static String response = String.Empty;

    private static void StartClient() {
        // Connect to a remote device.  
        try {
            // Establish the remote endpoint for the socket.  
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), SESSION_SERVER_PORT);

            // Create a TCP/IP socket.  
            Socket client = new Socket(remoteEP.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.  
            client.BeginConnect(remoteEP,
                new AsyncCallback(ConnectCallback), client);
            connectDone.WaitOne();

            Task readTask = new Task(() => {
                while (true) {
                    var chat = Console.ReadKey();
                    if (chat.Key == ConsoleKey.UpArrow) {
                        Send(client, new Login {
                            PID = "antori",
                            LoginAt = 1234,
                        });
                        ;
                    }
                }
            });
            readTask.Start();

            // Receive the response from the remote device.  
            Receive(client);

            while (true) {
            }
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }


    private static void ConnectCallback(IAsyncResult ar) {
        try {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.  
            client.EndConnect(ar);

            Console.WriteLine("Socket connected to {0}",
                client.RemoteEndPoint.ToString());

            // Signal that the connection has been made.  
            connectDone.Set();
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Receive(Socket client) {
        try {
            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = client;

            // Begin receiving the data from the remote device.  
            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void ReceiveCallback(IAsyncResult ar) {
        try {
            // Retrieve the state object and the client socket
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;

            // Read data from the remote device.  
            int bytesRead = client.EndReceive(ar);

            if (bytesRead > 0) {
                // There might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // All the data has arrived; put it in response.  
                if (state.sb.Length > 1) {
                    response = state.sb.ToString();

                    // Write the response to the console.  
                    Console.WriteLine("Response received : {0}", response);

                    state.buffer = new byte[StateObject.BufferSize];
                    state.sb.Clear();
                }
            }

            client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
new AsyncCallback(ReceiveCallback), state);
        } catch (Exception e) {
            Console.WriteLine(e.ToString());
        }
    }

    private static void Send(Socket client, IProtocol protocol) {
        // Convert the string data to byte data using ASCII encoding.  
        using (NetworkStream ns = new NetworkStream(client)) {
            using (BinaryWriter bw = new BinaryWriter(ns)) {
                protocol.Write(bw);

                byte[] temp = new byte[protocol.GetPacketLength()];
                ns.Write(temp);
            }
        }
    }

    public static int Main(String[] args) {
        StartClient();
        return 0;
    }
}