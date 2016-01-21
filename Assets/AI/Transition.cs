using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Transition : MonoBehaviour
{
    State targetState;
    List<Action> actions;
    Condition condi;
    int minVal;
    int maxVal;
    int curVal;
    bool pressed;
    float curTime;
    float maxTime;

    // type 0 for min-max check
    // type 1 for button press check
    // type 2 for time check
    int transType;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Transition(State target, List<Action> actList, Condition c)
    {
        targetState = target;
        actions = actList;
        condi = c;
    }

    public bool isTriggered()
    {
        return condi.Test();
    }

    public State getTargetState()
    {
        return targetState;
    }

    public List<Action> getAction()
    {
        return actions;
    }
}
