using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

class PlayerCharacterController : IController {
    public List<PlayerCharacter> pcList = new List<PlayerCharacter>();


    public void Update() {

    }

    public void CreatePlayerCharacter(Vector2 start_pos) {
        PlayerCharacter pc = new PlayerCharacter(100, 10, 2, start_pos);
    }
}