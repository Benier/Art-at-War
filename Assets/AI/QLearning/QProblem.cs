﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QProblem
{
    List<QState> states;
    AttackCondition attCon;
    QState middleState;
    QState nwCornerState;
    QState neCornerState;
    QState swCornerState;
    QState seCornerState;
    public QProblem()
    {
        states = new List<QState>();
        attCon = new AttackCondition();
        middleState = new QState("Middle");
        middleState.AddAction(new WanderNEAction());
        middleState.AddAction(new WanderNWAction());
        middleState.AddAction(new WanderSEAction());
        middleState.AddAction(new WanderSWAction());
        middleState.AddAction(new AttackNEAction());
        middleState.AddAction(new AttackNWAction());
        middleState.AddAction(new AttackSEAction());
        middleState.AddAction(new AttackSWAction());

        nwCornerState = new QState("NW Corner");
        nwCornerState.AddAction(new WanderNEAction());
        nwCornerState.AddAction(new WanderNWAction());
        nwCornerState.AddAction(new WanderSEAction());
        nwCornerState.AddAction(new WanderSWAction());
        nwCornerState.AddAction(new AttackNEAction());
        nwCornerState.AddAction(new AttackNWAction());
        nwCornerState.AddAction(new AttackSEAction());
        nwCornerState.AddAction(new AttackSWAction());

        neCornerState = new QState("NE Corner");
        neCornerState.AddAction(new WanderNEAction());
        neCornerState.AddAction(new WanderNWAction());
        neCornerState.AddAction(new WanderSEAction());
        neCornerState.AddAction(new WanderSWAction());
        neCornerState.AddAction(new AttackNEAction());
        neCornerState.AddAction(new AttackNWAction());
        neCornerState.AddAction(new AttackSEAction());
        neCornerState.AddAction(new AttackSWAction());

        swCornerState = new QState("SW Corner");
        swCornerState.AddAction(new WanderNEAction());
        swCornerState.AddAction(new WanderNWAction());
        swCornerState.AddAction(new WanderSEAction());
        swCornerState.AddAction(new WanderSWAction());
        swCornerState.AddAction(new AttackNEAction());
        swCornerState.AddAction(new AttackNWAction());
        swCornerState.AddAction(new AttackSEAction());
        swCornerState.AddAction(new AttackSWAction());

        seCornerState = new QState("SE Corner");
        seCornerState.AddAction(new WanderNEAction());
        seCornerState.AddAction(new WanderNWAction());
        seCornerState.AddAction(new WanderSEAction());
        seCornerState.AddAction(new WanderSWAction());
        seCornerState.AddAction(new AttackNEAction());
        seCornerState.AddAction(new AttackNWAction());
        seCornerState.AddAction(new AttackSEAction());
        seCornerState.AddAction(new AttackSWAction());

        states.Add(middleState);
        states.Add(nwCornerState);
        states.Add(neCornerState);
        states.Add(swCornerState);
        states.Add(seCornerState);
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
