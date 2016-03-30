using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QProblem
{
    List<QState> states;
    public QState GetRandomState()
    {
        int randInd = Random.Range(0, states.Count - 1);
        return states[randInd];
    }

    public List<QAction> GetAvailableActions(QState s)
    {
        return s.Actions;
    }

    public StateRewardPair takeAction(QState s, QAction a)
    {
        a.Execute();
        return new StateRewardPair(s, 0); //temp
    }
}
