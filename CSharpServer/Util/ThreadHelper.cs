using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class ThreadHelper : Singleton<ThreadHelper> {
    public void RegisterWork(Action cb) {

        ThreadPool.QueueUserWorkItem(new WaitCallback((object a) => {
            //Console.WriteLine("Thread ID : {0}", Thread.CurrentThread.ManagedThreadId);
            cb();
        }));
    }
}

