using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class FieldController : TickBase {
    List<TcpSessionHandler> clientList = new List<TcpSessionHandler>();
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
    public FieldController() {
        npcController = new NPCController(this, startPoint);
        controllerList.Add(npcController);

        playerCharacterController = new PlayerCharacterController(this, startPoint);
        controllerList.Add(playerCharacterController);

        Update();
    }

    #endregion

    #region Field Logic
    new public void Update() {
        base.Update();

        foreach (var con in controllerList) {
            con.Update();
        };
    }

    #endregion

    #region Session
    public void AddClient(TcpSessionHandler_Battle client) {
        clientList.Add(client);

        Character character = playerCharacterController.CreateCharacter(startPoint);

        if(character is PlayerCharacter) {
            client.PlayerCharacter = character as PlayerCharacter;
        } else {
            Console.WriteLine("Player Character Create Error!");
        }
    }

    public void RemoveClient(TcpSessionHandler_Battle client) {
        clientList.Remove(client);
        client.PlayerCharacter = null;
    }

    public void SendPacketField(IProtocol protocol) {
        foreach(var client in clientList) {
            client.SendPacket(protocol);
        }
    }

    #endregion
}
