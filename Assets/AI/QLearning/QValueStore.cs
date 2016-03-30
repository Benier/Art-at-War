using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QValueStore : MonoBehaviour
{
    public List<StateActionPair> store;
    List<QAction> possibleActions;
	// Use this for initialization
	void Start () {
        store = new List<StateActionPair>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AddPossibleAction(QAction a)
    {
        possibleActions.Add(a);
    }

    public float GetQValue(StateActionPair sap)
    {
        return sap.qVal;
    }

    public QAction GetBestAction(QState s)
    {
        float maxQ = 0;
        QAction bestAction = new QAction();
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

    public void storeQValue(QState s, QAction a, float q)
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
