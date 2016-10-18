﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QValueStore : MonoBehaviour
{
    public List<StateActionPair> store;
    public List<Action> possibleActions;
	// Use this for initialization
    void Awake()
    {
        StateActionPair sap;
        possibleActions = new List<Action>();
        List<QState> stateList = new List<QState>();
        QState middleState = new QState("Middle");
        QState nwCornerState = new QState("NW Corner");
        QState neCornerState = new QState("NE Corner");
        QState swCornerState = new QState("SW Corner");
        QState seCornerState = new QState("SE Corner");
        stateList.Add(middleState);
        stateList.Add(nwCornerState);
        stateList.Add(neCornerState);
        stateList.Add(swCornerState);
        stateList.Add(seCornerState);
        WanderNEAction wanNE = new WanderNEAction();
        WanderNWAction wanNW = new WanderNWAction();
        WanderSEAction wanSE = new WanderSEAction();
        WanderSWAction wanSW = new WanderSWAction();
        AttackNEAction attNE = new AttackNEAction();
        AttackNWAction attNW = new AttackNWAction();
        AttackSEAction attSE = new AttackSEAction();
        AttackSWAction attSW = new AttackSWAction();
        TarNEAction tarNE = new TarNEAction();
        TarNWAction tarNW = new TarNWAction();
        TarSEAction tarSE = new TarSEAction();
        TarSWAction tarSW = new TarSWAction();
        possibleActions.Add(wanNE);
        possibleActions.Add(attNE);
        possibleActions.Add(tarNE);
        possibleActions.Add(wanNW);
        possibleActions.Add(attNW);
        possibleActions.Add(tarNW);
        possibleActions.Add(wanSE);
        possibleActions.Add(attSE);
        possibleActions.Add(tarSE);
        possibleActions.Add(wanSW);
        possibleActions.Add(attSW);
        possibleActions.Add(tarSW);
        store = new List<StateActionPair>();

        foreach (QState s in stateList)
        {
            foreach (Action a in possibleActions)
            {
                sap = new StateActionPair(s, a);
                //if(att.Equals(a))
                //{
                //    sap.qVal = 2;
                //}
                store.Add(sap);
            }
        }
    }

	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void AddPossibleAction(Action a)
    {
        possibleActions.Add(a);
    }

    public float GetQValue(StateActionPair sap)
    {
        foreach(StateActionPair p in store)
        {
            if(p.state.statename == sap.state.statename && p.action.GetType() == sap.action.GetType())
            {
                return p.qVal;
            }
        }
        return sap.qVal;
    }

    public Action GetBestAction(QState s)
    {
        float maxQ = 0;
        Action bestAction = new WanderAction();
        foreach (StateActionPair p in store)
        {
            if(p.state.statename == s.statename && p.qVal >= maxQ)
            {
                maxQ = p.qVal;
                bestAction = p.action;
            }
        }

        return bestAction;
    }

    public void storeQValue(QState s, Action a, float q)
    {
        foreach(StateActionPair p in store)
        {
            if(p.state.statename == s.statename && p.action.GetType() == a.GetType())
            {
                p.qVal = q;
            }
        }
    }

    public List<StateActionPair> GetAllStateActionPairs()
    {
        return store;
    }

    public void zeroOutValues()
    {
        foreach (StateActionPair p in store)
        {
            p.qVal = 0;
        }
    }

    public void copyToStore(QValueStore target)
    {
        foreach (StateActionPair p in store)
        {
            foreach (StateActionPair tarp in target.store)
            {
                if (p.state.statename == tarp.state.statename && p.action.GetType() == tarp.action.GetType())
                {
                    tarp.qVal = p.qVal;
                }
            }
        }
    }
}
