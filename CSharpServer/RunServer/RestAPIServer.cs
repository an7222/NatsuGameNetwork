using System;
using System.Collections.Generic;
using System.Text;

class RestAPIServer : Singleton<RestAPIServer>, IRunServer {
    public void Start() {
        Console.WriteLine("RestAPI Server Start");
    }
}