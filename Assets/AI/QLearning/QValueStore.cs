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
        possibleActions.Add(wanNE);
        possibleActions.Add(wanNW);
        possibleActions.Add(wanSE);
        possibleActions.Add(wanSW);
        possibleActions.Add(attNE);
        possibleActions.Add(attNW);
        possibleActions.Add(attSE);
        possibleActions.Add(attSW);
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
