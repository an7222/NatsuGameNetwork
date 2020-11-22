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
        if (false == ActionQueue.IsEmpty) {
            Action action = DequeueAction();
            if (action != null)
                action();
        }
    }
    public void EnqueueAction(Action action) {
        ActionQueue.Enqueue(action);
    }

    protected Action DequeueAction() {
        Action cb;
        ActionQueue.TryDequeue(out cb);

        return cb;
    }
}