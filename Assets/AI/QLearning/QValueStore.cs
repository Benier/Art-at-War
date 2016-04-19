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
        QState move = new QState("Move");
        QState attack = new QState("Attack");
        stateList.Add(move);
        stateList.Add(attack);
        possibleActions.Add(new AttackAction());
        possibleActions.Add(new WanderAction());
        store = new List<StateActionPair>();

        foreach(QState s in stateList)
        {
            foreach(Action a in possibleActions)
            {
                sap = new StateActionPair(s, a);
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
            if(p.state == sap.action && p.action == sap.action)
            {
                return p.qVal;
            }
        }
        return sap.qVal;
    }

    public Action GetBestAction(QState s)
    {
        float maxQ = 0;
        Action bestAction = new PrintAction("empty action");
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
            else
            {
                StateActionPair sap = new StateActionPair(s, a);
                sap.qVal = q;
            }
        }
    }
}
