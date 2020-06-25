using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class NPCController : Controller{
    public List<NPC> npcList = new List<NPC>();
    public NPCController(Vector2 startPoint) {
        //temp monster
        CreateNPC(startPoint, NpcFightType.FIGHT);
    }
    new public void Update() {
        base.Update();

        foreach(var npc in npcList) {
            npc.ProcessFSM();
        }
    }

    public void CreateNPC(Vector2 start_pos, NpcFightType npcFightType) {
        NPC npc = new NPC(100, 10, 2, start_pos, npcFightType);
    }
}
