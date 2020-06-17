using System;
using System.Collections.Generic;
using System.Text;

class RunServerManager {
}

interface IRunServer {
    public void OnClientLeave(int session_id);
}
