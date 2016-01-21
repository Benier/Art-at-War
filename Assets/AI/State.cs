using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Interface for all states to implement
/// </summary>
public class State : MonoBehaviour
{
    List<Action> actions = new List<Action>();
    List<Action> entActions = new List<Action>();
    List<Action> extActions = new List<Action>();
    List<Transition> transitions = new List<Transition>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public State()
    {

    }

    public void addAction(Action a)
    {
        actions.Add(a);
    }

    public List<Action> getAction()
    {
        return actions;
    }

    public void addEntryAction(Action a)
    {
        entActions.Add(a);
    }

    public List<Action> getEntryAction()
    {
        return entActions;
    }

    public void addExitAction(Action a)
    {
        extActions.Add(a);
    }

    public List<Action> getExitAction()
    {
        return extActions;
    }

    public void addTransition(Transition t)
    {
        transitions.Add(t);
    }

    public List<Transition> getTransitions()
    {
        return transitions;
    }
}
