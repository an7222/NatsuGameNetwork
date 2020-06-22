using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Transactions;

class NPC : Character {
    int Aggro;

    Random r;
    public NPC(int hp, int attack, int def, Vector2 pos, bool FSM) : base(hp, attack, def, pos) {
        if (FSM) {
            r = new Random();
            ProcessFSM();
        }
    }

    //TODO FSM;
    public void ProcessFSM() {
        while (true) {
            Vector2 dest = new Vector2 {
                X = r.Next(-1, 1),
                Y = r.Next(-1, 1),
            };
            MoveTo(dest);

            FindEnemy();
        }
    }

    new public void ReceiveATtack(Character attacker) {
        base.ReceiveATtack(attacker);
        Aggro++;
    }

    public void FindEnemy() {

    }
}
