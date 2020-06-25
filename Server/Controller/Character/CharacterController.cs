using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

abstract class CharacterController : Controller {
    public abstract void CreateCharacter(Vector2 startPoint);
    public abstract void HandleDeadEvent(Character character);
}
