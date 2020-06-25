using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class NPCController : CharacterController {
    public List<NPC> npcList = new List<NPC>();

    Vector2 startPoint;
    public NPCController(Vector2 startPoint) {
        this.startPoint = startPoint;


        CreateCharacter(startPoint);
    }

    new public void Update() {
        base.Update();

        foreach (var npc in npcList) {
            npc.ProcessFSM();
        }
    }

    public override void CreateCharacter(Vector2 startPoint) {
        STAT stat = new STAT {
            HP = 100,
            ATTACK = 10,
            DEF = 10,
            SPEED = 1,
        };
        NPC npc = new NPC(stat, startPoint, NpcFightType.FIGHT);

        npc.CharacterController = this;

        npcList.Add(npc);
    }

    public override void HandleDeadEvent(Character character) {
        if (false == character is NPC) {
            Console.WriteLine("ERROR");
            return;
        }

        var npc = character as NPC;
        npcList.Remove(npc);

        Console.WriteLine("NPC Dead");

        CreateCharacter(startPoint);
    }
}
