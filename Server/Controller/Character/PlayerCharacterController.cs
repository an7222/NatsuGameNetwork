using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;

class PlayerCharacterController : CharacterController {


    public PlayerCharacterController(FieldController fc, Vector2 startPoint) : base(fc, startPoint) {
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

        characterList.Add(pc);
    }

    public override void HandleDeadEvent(Character character) {
        if (false == character is PlayerCharacter) {
            Console.WriteLine("ERROR");
            return;
        }

        var pc = character as PlayerCharacter;
        characterList.Remove(pc);

        Console.WriteLine("PC Dead");
    }
}