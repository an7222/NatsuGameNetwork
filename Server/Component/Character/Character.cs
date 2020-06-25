using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class Stat {
    public int HP {
        get;
        set;
    }
    public int ATTACK {
        get;
        set;
    }
    public int DEF {
        get;
        set;
    }
    public int SPEED {
        get;
        set;
    }
}

abstract class Character : GameObject {
    protected Vector2 pos;

    Stat stat;

    public Character(Stat stat, Vector2 pos) {
        this.stat = stat;
        this.pos = pos;
    }

    public void MoveTo(Vector2 dest) {
        pos = dest;
    }

    public void AttackTo(Character target) {
        target.ReceiveAttack(this);
    }

    public void ReceiveAttack(Character attacker) {
        ReceiveDamage(attacker.stat.ATTACK - this.stat.DEF);
    }

    void ReceiveDamage(int damage) {
        ChangeHP(damage);
    }

    void ChangeHP(int amount_change) {
        stat.HP += amount_change;
    }

    abstract public void OnDead();
}

