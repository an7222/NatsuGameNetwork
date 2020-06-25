using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class FieldController : Controller {
    List<TcpSessionHandler> clientList = new List<TcpSessionHandler>();
    public int FIELD_ID = 0;

    List<Controller> controllerList = new List<Controller>();
    public NPCController npcController {
        get;
        private set;
    }
    public PlayerCharacterController playerCharacterController {
        get;
        private set;
    }

    Vector2 startPoint = new Vector2 {
        X = 0,
        Y = 0
    };

    #region Controller Initialize
    public FieldController() {
        npcController = new NPCController(startPoint);
        controllerList.Add(npcController);

        playerCharacterController = new PlayerCharacterController(startPoint);
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
        client.SetPlayerCharacterController(playerCharacterController);
    }

    public void RemoveClient(TcpSessionHandler_Battle client) {
        clientList.Remove(client);
        client.RemovePlayerCharacterController();
    }

    public void SendPacketField(IProtocol protocol) {
        foreach(var client in clientList) {
            client.SendPacket(protocol);
        }
    }

    #endregion
}
