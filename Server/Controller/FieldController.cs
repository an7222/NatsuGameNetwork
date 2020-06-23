using System;
using System.Collections.Generic;
using System.Text;

class FieldController : Controller {
    List<TcpSessionHandler> clientList = new List<TcpSessionHandler>();
    public int FIELD_ID = 0;

    List<Controller> controllerList = new List<Controller>();
    NPCController npcController;
    PlayerCharacterController playerCharacterController;

    #region Controller Initialize
    public FieldController() {
        npcController = new NPCController();
        controllerList.Add(npcController);

        playerCharacterController = new PlayerCharacterController();
        controllerList.Add(playerCharacterController);

        Update();
    }

    public NPCController GetNPCController() {
        return npcController;
    }

    public PlayerCharacterController GetPlayerCharacterController() {
        return playerCharacterController;
    }

    #endregion

    #region Field Logic
    new public void Update() {
        base.Update();

        foreach (var con in controllerList) {
            con.Update();
        };
    }

    public void CreateMonster() {

    }


    #endregion

    #region Session
    public void AddClient(TcpSessionHandler_Battle client) {
        clientList.Add(client);
        client.SetFieldController(this);
    }

    public void RemoveClient(TcpSessionHandler_Battle client) {
        clientList.Remove(client);
        client.SetFieldController(null);
    }

    public void SendPacketField(IProtocol protocol) {
        foreach(var client in clientList) {
            client.SendPacket(protocol);
        }
    }

    #endregion
}
