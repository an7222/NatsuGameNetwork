using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class Character {
    protected int hp;
    protected int attack;
    protected int def;

    protected Vector2 pos;

    public Character(int hp, int attack, int def, Vector2 pos) {
        this.hp = hp;
        this.attack = attack;
        this.def = def;
        this.pos = pos;
    }

    public void MoveTo(Vector2 dest) {
        pos = dest;
    }

    public void AttackTo(Character target) {
        target.ReceiveATtack(this);
    }

    public void ReceiveATtack(Character attacker) {
        ReceiveDamage(attacker.attack - this.def);
    }

    protected void ReceiveDamage(int damage) {
        ChangeHp(damage);
    }

    protected void ChangeHp(int amount_change) {
        hp += amount_change;
    }
}

