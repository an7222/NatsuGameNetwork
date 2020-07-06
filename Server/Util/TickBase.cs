using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class TickBase {
    Queue<Action> ActionQueue = new Queue<Action>();

    protected Stopwatch sw = new Stopwatch();

    public void Update() {
        Action action = DequeueMesssage();
        if (action != null)
            action();
    }
    public void EnqueueMesssage(Action action) {
        ActionQueue.Enqueue(action);
    }

    protected Action DequeueMesssage() {
        if(ActionQueue.Count > 0) {
            return ActionQueue.Dequeue();
        }

        return null;
    }
}