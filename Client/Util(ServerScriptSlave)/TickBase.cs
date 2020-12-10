using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class TickBase {
    ConcurrentQueue<Action> ActionQueue = new ConcurrentQueue<Action>();

    protected Stopwatch sw = new Stopwatch();

    public bool updateLock {
        get; set;
    }

    public virtual void Update() {
        while (false == ActionQueue.IsEmpty) {
            if (ActionQueue.TryDequeue(out var cb)) {
                cb();
            }
        }
    }
    public void EnqueueAction(Action action) {
        ActionQueue.Enqueue(action);
    }
}