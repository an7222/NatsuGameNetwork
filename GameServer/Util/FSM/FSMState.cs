using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

abstract class FSMState<T> {
    protected Stopwatch sw = new Stopwatch();
    public abstract void Enter(T entity);
    public abstract void Update(T entity);
    public abstract void Exit(T entity);
}