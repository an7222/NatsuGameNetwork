using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class NPC_MoveState : FSMState<NPC> {
    Random r;
    public override void Enter(NPC npc) {
        npc.dir = (Direction)r.Next(0, 4);
        npc.isMoving = true;
        Console.Write("[Move] Enter");
    }
    public override void Update(NPC npc) {
        if (sw.Elapsed.TotalSeconds >= 1) {
            npc.FSM.ChangeState(npc.IdleState);
        }
        //TODO find Enemy
        Console.Write("[Move] Update");
    }
    public override void Exit(NPC npc) {
        npc.isMoving = false;
        Console.Write("[Move] Exit");
    }
}
