using System;
class Program {
    static void Main(string[] args) {
        ThreadManager.GetInstance().RegisterWork(() => {
            Console.WriteLine("Test");
        });

        
    }
}
