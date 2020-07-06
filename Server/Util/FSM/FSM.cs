using System;
using System.Collections.Generic;
using System.Text;

class FSM<T> {
    private T Owner;
    private FSMState<T> CurrentState;
    private FSMState<T> PreviousState;

    public void Configure(T owner, FSMState<T> InitialState) {
        Owner = owner;
        ChangeState(InitialState);
    }

    public void Update() {
        if (CurrentState != null)
            CurrentState.Update(Owner);
    }

    public void ChangeState(FSMState<T> NewState) {
        PreviousState = CurrentState;
        if (CurrentState != null)
            CurrentState.Exit(Owner);
        CurrentState = NewState;
        if (CurrentState != null)
            CurrentState.Enter(Owner);
    }

    public void RevertToPreviousState() {
        if (PreviousState != null)
            ChangeState(PreviousState);
    }
}
