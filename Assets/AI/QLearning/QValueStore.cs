using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QValueStore : MonoBehaviour
{
    public List<StateActionPair> store;
    public List<Action> possibleActions;
	// Use this for initialization
	void Start () {
        store = new List<StateActionPair>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddPossibleAction(Action a)
    {
        possibleActions.Add(a);
    }

    public float GetQValue(StateActionPair sap)
    {
        return sap.qVal;
    }

    public Action GetBestAction(QState s)
    {
        float maxQ = 0;
        Action bestAction = new PrintAction("empty action");
        foreach (StateActionPair p in store)
        {
            if(p.state == s && p.qVal >= maxQ)
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
            if(p.state == s && p.action == a)
            {
                p.qVal = q;
            }
        }
    }
}
