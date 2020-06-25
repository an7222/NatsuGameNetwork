using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class PlayerCharacterController : Controller {
    public List<PlayerCharacter> pcList = new List<PlayerCharacter>();

    public PlayerCharacterController(Vector2 startPoint) {
    }

    public void CreatePlayerCharacter(Vector2 start_pos) {
        STAT stat = new STAT {
            HP = 100,
            ATTACK = 10,
            DEF = 10,
            SPEED = 1,
        };

        PlayerCharacter pc = new PlayerCharacter(stat, start_pos);
    }
}