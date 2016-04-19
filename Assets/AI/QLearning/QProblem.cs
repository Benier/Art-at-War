using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QProblem
{
    List<QState> states;

    public QProblem()
    {
        states = new List<QState>();
        QState moveState = new QState("Move");
        moveState.AddAction(new WanderAction());
        QState attackState = new QState("Attack");
        attackState.AddAction(new AttackAction());

        states.Add(moveState);
        states.Add(attackState);
    }

    public QState GetRandomState()
    {
        int randInd = Random.Range(0, states.Count - 1);
        return states[randInd];
    }

    public List<Action> GetAvailableActions(QState s)
    {
        return s.actions;
    }

    public StateRewardPair takeAction(QState s, Action a, Unit u)
    {
        a.Execute(u);
        return new StateRewardPair(s, a.GetReward()); 
    }
}
