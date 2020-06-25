using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class PlayerCharacterController : CharacterController {
    public List<PlayerCharacter> pcList = new List<PlayerCharacter>();

    Vector2 startPoint;
    public PlayerCharacterController(Vector2 startPoint) {
        this.startPoint = startPoint;

        CreateCharacter(startPoint);
    }

    public override void CreateCharacter(Vector2 startPoint) {
        STAT stat = new STAT {
            HP = 100,
            ATTACK = 10,
            DEF = 10,
            SPEED = 1,
        };

        PlayerCharacter pc = new PlayerCharacter(stat, startPoint);
        pc.CharacterController = this;

        pcList.Add(pc);
    }

    public override void HandleDeadEvent(Character character) {
        if (false == character is PlayerCharacter) {
            Console.WriteLine("ERROR");
            return;
        }

        var pc = character as PlayerCharacter;
        pcList.Remove(pc);

        Console.WriteLine("PC Dead");

        CreateCharacter(startPoint);
    }
}