using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// Transition class that tests Conditions and return target State and Action.
/// </summary>
public class Transition 
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
    TextureGenerator texGen;

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

    public Transition(State target, List<Action> actList, Condition c, TextureGenerator tg)
    {
        targetState = target;
        actions = actList;
        condi = c;
        texGen = tg;
    }

    public bool isTriggered(Unit u)
    {
        return condi.Test(u, texGen);
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
