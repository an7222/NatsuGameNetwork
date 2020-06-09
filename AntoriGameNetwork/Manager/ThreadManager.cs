using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class ThreadManager : Singleton<ThreadManager> {
    void RegisterWork(Action cb) {

        ThreadPool.QueueUserWorkItem(new WaitCallback((object a) => {
            cb();
        }));
    }
}

