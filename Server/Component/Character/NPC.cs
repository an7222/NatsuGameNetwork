using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

class AggroInstance {
    int OBJECT_ID;
    int aggro;
    DateTime aggroClearTime;
    public AggroInstance(int object_ID, int aggro, DateTime aggroClearTime) {
        this.OBJECT_ID = object_ID;
        this.aggro = aggro;
        this.aggroClearTime = aggroClearTime;
    }

    public void UpdateAggro(int changeAmout) {
        aggro += changeAmout;
    }
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
    public NPC(STAT stat, Vector2 pos, NpcFightType npcFightType) : base(stat, pos) {
        this.npcFightType = npcFightType;

        ProcessFSM();
    }

    //TODO FSM;
    public void ProcessFSM() {
        r = new Random();

        //TODO pos changed check
        MoveTo(new Vector2(pos.X += r.Next(-1, 1), pos.Y += r.Next(-1, 1)));

        FindEnemy();
    }

    new public void ReceiveAttack(Character attacker) {
        base.ReceiveAttack(attacker);

        if (npcFightType != NpcFightType.FIGHT)
            return;

        //Aggro Process
        int attackerObjectID = attacker.OBJECT_ID;
        if (aggroMap.ContainsKey(attackerObjectID)) {
            aggroMap[attackerObjectID].UpdateAggro(1);
        } else {
            aggroMap.Add(attackerObjectID, new AggroInstance(attackerObjectID, 1, DateTime.Now.AddSeconds(Const.AGGRO_CLEAR_SEC)));
        }
    }

    public override void OnDead() {

    }

    void FindEnemy() {
        if (npcFightType == NpcFightType.PIECE)
            return;
    }

    void ClearAggro(Character enemy) {
        int attackerObjectID = enemy.OBJECT_ID;
        if (aggroMap.ContainsKey(attackerObjectID)) {
            aggroMap.Remove(attackerObjectID);
        }
    }

    public void Chase(Character target) {

    }

    public void Escape(Character target) {

    }
}
