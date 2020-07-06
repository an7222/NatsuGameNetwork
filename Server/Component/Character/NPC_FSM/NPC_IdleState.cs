using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class NPC_IdleState : FSMState<NPC> {
    public override void Enter(NPC npc) {
        sw.Start();
        Console.Write("[Idle] Enter");
    }
    public override void Update(NPC npc) {
        if(sw.Elapsed.TotalSeconds >= 3) {
            npc.FSM.ChangeState(npc.MoveState);
        }
        Console.Write("[Idle] Update");
    }
    public override void Exit(NPC npc) {
        if (sw.IsRunning) {
            sw.Stop();
        }
        Console.Write("[Idle] Exit");
    }
}
