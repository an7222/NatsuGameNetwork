using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class TickBase {
    ConcurrentQueue<Action> ActionQueue = new ConcurrentQueue<Action>();

    protected Stopwatch sw = new Stopwatch();

    public virtual void Update() {
        Action cb;
        if (ActionQueue.TryDequeue(out cb)) {
            cb();
        }
    }
    public void EnqueueAction(Action action) {
        ActionQueue.Enqueue(action);
    }
}