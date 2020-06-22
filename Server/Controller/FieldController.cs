using System;
using System.Collections.Generic;
using System.Text;

class FieldController : Controller {
    List<TcpSessionHandler> clientList = new List<TcpSessionHandler>();
    public int FIELD_ID = 0;
    class FieldInstance {
    }

    List<Controller> controllerList = new List<Controller>();
    NPCController npcController;
    PlayerCharacterController playerCharacterController;

    public FieldController() {
        FieldInstance fieldInstance = new FieldInstance();

        npcController = new NPCController();
        controllerList.Add(npcController);

        playerCharacterController = new PlayerCharacterController();
        controllerList.Add(playerCharacterController);


        Update();
    }

    new public void Update() {
        base.Update();

        foreach (var con in controllerList) {
            con.Update();
        };
    }

    public void AddClient(TcpSessionHandler client) {
        clientList.Add(client);
    }

    public void RemoveClient(TcpSessionHandler client) {
        clientList.Remove(client);
    }

    public void SendPacketField(IProtocol protocol) {
        foreach(var client in clientList) {
            client.SendPacket(protocol);
        }
    }
}
