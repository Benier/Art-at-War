using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QProblem
{
    List<QState> states;

    public QProblem()
    {
        states = new List<QState>();
        states.Add(new QState("Move N 1AP"));
    }

    public QState GetRandomState()
    {
        int randInd = Random.Range(0, states.Count - 1);
        return states[randInd];
    }

    public List<QAction> GetAvailableActions(QState s)
    {
        return s.actions;
    }

    public StateRewardPair takeAction(QState s, QAction a, Unit u)
    {
        a.Execute(u);
        return new StateRewardPair(s, a.GetReward()); 
    }
}
