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
using System.Net;

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
                var httpClient = new HttpClient();
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.BaseAddress = new Uri("http://localhost:" + Const.REST_API_SERVER_PORT + "/weatherforecast");

                RestAPI_REQ(httpClient, handler);
            };
        }
        return action;
    }

    async void RestAPI_REQ(HttpClient httpClient, TcpHandler handler) {
        HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress);
        if (response.IsSuccessStatusCode) {
            var info = await response.Content.ReadAsStringAsync();
            handler.SendPacket(new RestAPI_RES_S2C {
                Info = info,
            });

            Console.WriteLine("Send : [RestAPI_RES_S2C]" + info);
        }
    }
}

