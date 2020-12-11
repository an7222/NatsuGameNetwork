using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using System.Text;

class NPCController : CharacterController {
    Random r;

    public NPCController(ZoneController cc, Vector2 startPoint) : base(cc, startPoint) {
        r = new Random();

        for(int i = 0; i < 5; ++i) {
            CreateCharacter(startPoint);
        }
    }

    public override void Update() {
        base.Update();

        foreach (var character in characterList) {
            if (character is NPC) {
                var npc = character as NPC;
                npc.FSM.Update();
            }
        }
    }

    public override Character CreateCharacter(Vector2 startPoint) {
        STAT stat = new STAT {
            HP = 100,
            ATTACK = 10,
            DEF = 10,
            SPEED = 1,
        };
        NPC npc = new NPC(stat, startPoint, NpcFightType.FIGHT);
        FSM<NPC> FSM = new FSM<NPC>();
        npc.FSM = FSM;
        npc.OBJECT_ID = zoneController.GetObjectID();
        FSM.Configure(npc, new NPC_IdleState());

        npc.CharacterController = this;

        characterList.Add(npc);

        return npc;
    }

    public override void HandleDeadEvent(Character character) {
        if (false == character is NPC) {
            Console.WriteLine("ERROR");
            return;
        }

        var npc = character as NPC;
        characterList.Remove(npc);

        Console.WriteLine("NPC Dead");
    }
}
