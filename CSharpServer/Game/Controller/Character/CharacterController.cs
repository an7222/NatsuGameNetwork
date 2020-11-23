using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;

abstract class CharacterController : TickBase {
    protected Vector2 startPoint;
    protected ChannelController channelController;
    public List<Character> characterList = new List<Character>();

    public CharacterController(ChannelController cc, Vector2 startPoint) {
        this.channelController = cc;
        this.startPoint = startPoint;
    }

    protected long elapsedTime = 0;

    public override void Update() {
        if (sw.IsRunning) {
            if (sw.ElapsedMilliseconds <= 0) {
                return;
            }

            elapsedTime = sw.ElapsedMilliseconds;
            sw.Stop();
        }

        sw.Start();
        foreach (var pc in characterList) {
            if (pc.isMoving) {
                if (pc.dir == Direction.Right) {
                    pc.SetPos(new Vector2(pc.pos.X + (elapsedTime / 1000) * pc.stat.SPEED, pc.pos.Y));
                } else if (pc.dir == Direction.Left) {
                    pc.SetPos(new Vector2(pc.pos.X - (elapsedTime / 1000) * pc.stat.SPEED, pc.pos.Y));
                } else if (pc.dir == Direction.Up) {
                    pc.SetPos(new Vector2(pc.pos.X, pc.pos.Y + (elapsedTime / 1000) * pc.stat.SPEED));
                } else if (pc.dir == Direction.Down) {
                    pc.SetPos(new Vector2(pc.pos.X, pc.pos.Y - (elapsedTime / 1000) * pc.stat.SPEED));
                }

                BroadCast_RefreshPos(pc);
            }
        }
    }

    public abstract Character CreateCharacter(Vector2 startPoint);
    public abstract void HandleDeadEvent(Character character);

    public void BroadCast_MoveStart(Character caster) {
        var protocol = new MoveStart_B2C {
            OBJECT_ID = caster.OBJECT_ID,
            Direction = (int)caster.dir,
        };

        caster.isMoving = true;

        channelController.SendPacketChannel(protocol);
    }

    public void BroadCast_MoveEnd(Character caster) {
        var protocol = new MoveEnd_B2C {
            OBJECT_ID = caster.OBJECT_ID,
        };

        caster.isMoving = false;

        channelController.SendPacketChannel(protocol);
    }

    public void BroadCast_RefreshPos(Character caster) {
        var protocol = new ChangePos_B2C {
            OBJECT_ID = caster.OBJECT_ID,
            Pos_x = caster.pos.X,
            Pos_y = caster.pos.Y,
        };

        channelController.SendPacketChannel(protocol);
    }

    //public void BroadCast_AttackTo(Character caster, Character target) {
    //    target.ReceiveAttack(this);
    //}

    //public void BroadCast_ReceiveAttack(Character attacker) {
    //    ReceiveDamage(attacker.stat.ATTACK - this.stat.DEF);
    //}

    //void BroadCast_ReceiveDamage(int damage) {
    //    ChangeHP(damage);
    //}

    //void BroadCast_ChangeHP(int amount_change) {
    //    stat.HP += amount_change;

    //    if (stat.HP <= 0)
    //        OnDead();
    //}
}
