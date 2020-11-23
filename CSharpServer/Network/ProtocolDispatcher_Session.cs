using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Net.Http;

partial class ProtocolDispatcher : Singleton<ProtocolDispatcher> {
    Action<IProtocol, TcpHandler> createAction_session(IProtocol dummyProtocol) {
        Action<IProtocol, TcpHandler> action = null;

        if (dummyProtocol is Login_REQ_C2S) {
            action = (IProtocol protocol, TcpHandler handler) => {
                var cast = protocol as Login_REQ_C2S;
                Console.WriteLine("Receive : [Login_REQ_C2S]");
                Random r = new Random();
                handler.SendPacket(new Login_RES_S2C {
                    USER_ID = SessionServer.GetInstance().GetUniqueUserID(),
                    ServerTimeUnix = DateTime.Now.Ticks,
                    SessionToken = Guid.NewGuid().ToString(),
                    CHANNEL_ID = r.Next(1, 2), //TODO read for RESTAPI
                });

                Console.WriteLine("Send : [Login_RES_S2C]");
            };
        } else if (dummyProtocol is Login_FIN_C2S) {
            action = (IProtocol protocol, TcpHandler handler) => {
                Console.WriteLine("Receive : [Login_FIN_C2S]");
                SessionServer.GetInstance().AddClient(handler);
            };
        } else if (dummyProtocol is RestAPI_REQ_C2S) {
            action = (IProtocol protocol, TcpHandler handler) => {
                Console.WriteLine("Receive : [RestAPI_REQ_C2S]");
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri("https://localhost:" + Const.REST_API_SERVER_PORT + "/weatherforecast");

                RestAPI_REQ(client, handler);
            };
        }
        return action;
    }

    async void RestAPI_REQ(HttpClient client, TcpHandler handler) {
        HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
        if (response.IsSuccessStatusCode) {
            var info = await response.Content.ReadAsStringAsync();
            handler.SendPacket(new RestAPI_RES_S2C {
                Info = info,
            });

            Console.WriteLine("Send : [RestAPI_RES_S2C]" + info);
        }
    }
}

