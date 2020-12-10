using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class NPC_MoveState : FSMState<NPC> {
    Random r = new Random();
    public override void Enter(NPC npc) {
        sw.Restart();
        npc.dir = (Direction)r.Next(0, 4);
        npc.isMoving = true;
    }
    public override void Update(NPC npc) {
        if (sw.Elapsed.TotalSeconds >= 5) {
            npc.FSM.ChangeState(npc.IdleState);
        }
        //TODO find Enemy
    }
    public override void Exit(NPC npc) {
        npc.isMoving = false;
        if (sw.IsRunning) {
            sw.Stop();
        }
    }
}
