using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;

partial class ProtocolHandler : Singleton<ProtocolHandler> {
    Action<IProtocol, TcpSessionHandler> createAction_battle(IProtocol dummyProtocol) {
        Action<IProtocol, TcpSessionHandler> action = null;

        if (dummyProtocol is NewBattleUser_REQ_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as NewBattleUser_REQ_C2B;
                Console.WriteLine("Receive : [NewBattleUser_REQ_C2B]");

                if (false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;
                battleHandler.SendPacket(new NewBattleUser_RES_C2B {
                    ObjectIDList = 4,
                    TODOStatusList = 5,
                });

                Console.WriteLine("Send : [NewBattleUser_RES_C2B]");
            };
        }
        if (dummyProtocol is NewBattleUser_FIN_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as NewBattleUser_FIN_C2B;
                Console.WriteLine("Receive : [NewBattleUser_FIN_C2B]");

                if (false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;

                Console.WriteLine("FIELD ID : " + cast.CHANNEL_ID);
                battleHandler.CHANNEL_ID = cast.CHANNEL_ID;
                BattleServer.GetInstance().AddClient(battleHandler);
            };
        } else if (dummyProtocol is MoveStart_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveStart_C2B;
                Console.WriteLine("Receive : [MoveStart_C2B]");

                if (false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;

                battleHandler.PlayerCharacter.MoveStart((Direction)cast.Direction);

                battleHandler.PlayerCharacter.CharacterController.BroadCast_MoveStart(battleHandler.PlayerCharacter);
            };
        } else if (dummyProtocol is MoveEnd_C2B) {
            action = (IProtocol protocol, TcpSessionHandler handler) => {
                var cast = protocol as MoveEnd_C2B;
                Console.WriteLine("Receive : [MoveEnd_C2B]");

                if (false == IsBattleHandler(handler)) {
                    Console.WriteLine("No Battle Handler!");
                    return;
                }

                var battleHandler = handler as TcpSessionHandler_Battle;

                BattleServer.GetInstance().SendPacketChannel(new MoveEnd_B2C {
                }, battleHandler.CHANNEL_ID);

                Console.WriteLine("SendField : [MoveEnd_B2C]");
            };
        }
        return action;
    }
}

