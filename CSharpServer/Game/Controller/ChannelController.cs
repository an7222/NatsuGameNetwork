using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

class ChannelController : TickBase {
    List<TcpHandler> clientList = new List<TcpHandler>();
    public int FIELD_ID = 0;

    List<TickBase> controllerList = new List<TickBase>();
    public NPCController npcController {
        get;
        private set;
    }
    public PlayerCharacterController playerCharacterController {
        get;
        private set;
    }

    //TODO : Load For Map Data
    Vector2 startPoint = new Vector2 {
        X = 0,
        Y = 0
    };

    #region Controller Initialize
    public ChannelController() {
        npcController = new NPCController(this, startPoint);
        controllerList.Add(npcController);

        playerCharacterController = new PlayerCharacterController(this, startPoint);
        controllerList.Add(playerCharacterController);
    }

    #endregion

    #region Channel Logic
    public override void Update() {
        foreach (var con in controllerList) {
            if (con.updateLock)
                continue;

            con.updateLock = true;
            ThreadManager.GetInstance().RegisterWork(() => {
                con.Update();
                con.updateLock = false;
            });
        };
    }

    #endregion

    #region Session
    public void AddClient(TcpHandler_Battle client) {
        clientList.Add(client);

        Character character = playerCharacterController.CreateCharacter(startPoint);

        if (character is PlayerCharacter) {
            client.PlayerCharacter = character as PlayerCharacter;
        } else {
            Console.WriteLine("Player Character Create Error!");
        }
    }

    public void RemoveClient(TcpHandler_Battle client) {
        clientList.Remove(client);
        client.PlayerCharacter = null;
    }

    public void SendPacketChannel(IProtocol protocol) {
        foreach (var client in clientList) {
            client.SendPacket(protocol);
        }
    }

    #endregion
}
