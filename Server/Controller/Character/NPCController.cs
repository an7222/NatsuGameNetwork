using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class NPCController : CharacterController {
    Random r;
    public NPCController(FieldController fc, Vector2 startPoint) : base(fc, startPoint){
        r = new Random();
    }

    new public void Update() {
        base.Update();

        foreach (var character in characterList) {
            if(character is NPC) {
                ProcessFSM(character as NPC);
            }
        }
    }

    //TODO FSM;
    public void ProcessFSM(NPC npc) {
        if(elapsedTime <= 0) {
            return;
        }

        npc.isMoving = true;
        SetPos(new Vector2(pos.X += r.Next(-1, 1), pos.Y += r.Next(-1, 1)));

        FindEnemy();
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

        characterList.Add(npc);
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
