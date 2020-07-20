using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class NPC_IdleState : FSMState<NPC> {
    public override void Enter(NPC npc) {
        sw.Restart();
        Console.WriteLine("[Idle] Enter");
    }
    public override void Update(NPC npc) {
        if(sw.Elapsed.TotalSeconds >= 3) {
            npc.FSM.ChangeState(npc.MoveState);
        }
    }
    public override void Exit(NPC npc) {
        if (sw.IsRunning) {
            sw.Stop();
        }
        Console.WriteLine("[Idle] Exit");
    }
}
