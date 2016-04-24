using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QProblem
{
    List<QState> states;
    AttackCondition attCon;
    QState barrenState;
    QState lushState;
    public QProblem()
    {
        states = new List<QState>();
        attCon = new AttackCondition();
        barrenState = new QState("Barren");
        barrenState.AddAction(new WanderAction());
        lushState = new QState("Lush");
        lushState.AddAction(new AttackAction());

        states.Add(barrenState);
        states.Add(lushState);
    }

    public QState GetRandomState()
    {
        int randInd = Random.Range(0, states.Count);
        return states[randInd];
    }

    public List<Action> GetAvailableActions(QState s)
    {
        return s.actions;
    }

    public StateRewardPair takeAction(QState s, Action a, Unit u, TextureGenerator tg)
    {
        QState newState;
        a.Execute(u);
        //if(attCon.GetPotential(u, tg) <= 0.35f)
        //{
        //    newState = barrenState;
        //}
        //else
        //{
        //    newState = lushState;
        //}
        return new StateRewardPair(s, a.GetReward()); 
    }
}
