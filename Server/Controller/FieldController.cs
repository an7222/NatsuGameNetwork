using System;
using System.Collections.Generic;
using System.Text;

class FieldController : IController {
    class FieldInstance {
    }

    List<IController> controllerList = new List<IController>();
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

    public void Update() {
        foreach (var con in controllerList) {
            con.Update();
        };
    }
}
