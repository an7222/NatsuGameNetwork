using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class PlayerCharacterController : Controller {
    public List<PlayerCharacter> pcList = new List<PlayerCharacter>();

    public void CreatePlayerCharacter(Vector2 start_pos) {
        PlayerCharacter pc = new PlayerCharacter(100, 10, 2, start_pos);
    }
}