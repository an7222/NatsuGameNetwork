using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace UnitTest {
    class Program {
        static void Main(string[] args) {
            TcpListener server = null;
            // Set the TcpListener on port 13000.
            Int32 port = 8001;
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            // Enter the listening loop.
            while (true) {
                Console.Write("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected!");

                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) {
                    using(BinaryReader br = new BinaryReader(stream)) {
                        // Translate data bytes to a ASCII string.
                        int data2 = br.ReadInt32();
                        Console.WriteLine("Received: {0}", data2);
                    }



                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    Console.WriteLine("Sent: {0}", data);
                }

                // Shutdown and end connection
                client.Close();
            }


        }
    }

}

