using System;
using System.Collections.Generic;
using System.Text;

class Controller {
    protected Queue<Action> MessageQueue = new Queue<Action>();
    public void Update() {
        Action action = DequeueMesssage();
        if (action != null)
            action();
    }
    public void EnqueueMesssage(Action action) {
        MessageQueue.Enqueue(action);
    }
    protected Action DequeueMesssage() {
        return MessageQueue.Dequeue();
    }
}