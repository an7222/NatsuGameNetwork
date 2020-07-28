using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace UnitTest {
    class Program {
        struct AggroInstance {
            int object_ID;
            public int aggro;
            DateTime aggroClearTime;

            public void UpdateAggro(int changeAmount) {
                aggro += changeAmount;
            }
        }

        static void Main(string[] args) {
            AggroInstance temp = new AggroInstance();
            Console.WriteLine(temp.aggro);
            temp.UpdateAggro(1);
            temp.aggro++;
            Console.WriteLine(temp.aggro);

        }
    }

}

