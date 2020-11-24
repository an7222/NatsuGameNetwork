using System;
using System.Collections.Generic;
using System.Text;

class Const {
    public const int BODY_BUFFER_SIZE = 256;
    public const int HEADER_BUFFER_SIZE = 4;

    public const int SESSION_SERVER_PORT = 8001;
    public const int BATTLE_SERVER_PORT = 8002;
    public const int REST_API_SERVER_PORT = 5000;

    public const int AGGRO_CLEAR_SEC = 10;
}

public enum Direction {
    Up,
    Down,
    Left,
    Right,
}