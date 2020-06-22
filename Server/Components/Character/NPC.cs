using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

class AggroInstance {
    public int object_ID;
    public int aggro;
    public DateTime aggroClearTime;
}

enum NpcFightType {
    PIECE,
    FIGHT,
    ONLY_ESCAPE,
}

class NPC : Character {
    Dictionary<int, AggroInstance> aggroMap = new Dictionary<int, AggroInstance>();

    Random r;

    NpcFightType npcFightType;
    public NPC(int HP, int attack, int def, Vector2 pos, NpcFightType npcFightType) : base(HP, attack, def, pos) {
        this.npcFightType = npcFightType;

        ProcessFSM();
    }

    //TODO FSM;
    public void ProcessFSM() {
        r = new Random();
        //TODO pos changed check
        pos.X += r.Next(-1, 1);
        pos.Y += r.Next(-1, 1);

        FindEnemy();
    }

    new public void ReceiveAttack(Character attacker) {
        base.ReceiveAttack(attacker);

        if (npcFightType != NpcFightType.FIGHT)
            return;

        //Aggro Process
        int attackerObjectID = attacker.GetObjectID();
        if (aggroMap.ContainsKey(attackerObjectID)) {
            aggroMap[attackerObjectID].aggro++;
        } else {
            aggroMap.Add(attackerObjectID, new AggroInstance {
                object_ID = attackerObjectID,
                aggro = 1,
                aggroClearTime = DateTime.Now.AddSeconds(Const.AGGRO_CLEAR_SEC),
            });
        }
    }

    void FindEnemy() {
        if (npcFightType == NpcFightType.PIECE)
            return;
    }

    void ClearAggro(Character enemy) {
        int attackerObjectID = enemy.GetObjectID();
        if (aggroMap.ContainsKey(attackerObjectID)) {
            aggroMap.Remove(attackerObjectID);
        }
    }

    public void Chase(Character target) {

    }

    public void Escape(Character target) {

    }
}
