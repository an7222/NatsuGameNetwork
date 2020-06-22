using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class Character : GameObject {
    protected int HP;
    protected int attack;
    protected int def;

    protected Vector2 pos;

    public Character(int HP, int attack, int def, Vector2 pos) {
        this.HP = HP;
        this.attack = attack;
        this.def = def;
        this.pos = pos;
    }

    public void MoveTo(Vector2 dest) {
        pos = dest;
    }

    public void AttackTo(Character target) {
        target.ReceiveAttack(this);
    }

    public void ReceiveAttack(Character attacker) {
        ReceiveDamage(attacker.attack - this.def);
    }

    protected void ReceiveDamage(int damage) {
        ChangeHP(damage);
    }

    protected void ChangeHP(int amount_change) {
        HP += amount_change;
    }
}

