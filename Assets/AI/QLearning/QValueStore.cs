using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QValueStore : MonoBehaviour
{
    public List<StateActionPair> store;
    public List<Action> possibleActions;
	// Use this for initialization
	void Start ()
    {
        StateActionPair sap;
        possibleActions = new List<Action>();
        List<QState> stateList = new List<QState>();
        QState barrenState = new QState("Barren");
        QState lushState = new QState("Lush");
        stateList.Add(barrenState);
        stateList.Add(lushState);
        AttackAction att = new AttackAction();
        WanderAction wan = new WanderAction();
        possibleActions.Add(att);
        possibleActions.Add(wan);
        store = new List<StateActionPair>();

        foreach(QState s in stateList)
        {
            foreach(Action a in possibleActions)
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
}
