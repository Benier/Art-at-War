using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine {

    ArrayList statesList = new ArrayList();
    State initialState;
    State currentState;
    State targetState;
    Transition triggeredTransition;
    List<Action> actions;
    Unit controlUnit;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    /// <summary>
    /// Constructor that sets current state to initial state
    /// </summary>
    /// <param name="init"></param>
    public StateMachine(State init, Unit u)
    {
        initialState = init;
        currentState = initialState;
        controlUnit = u;
    }

    /// <summary>
    /// Add State to state machine
    /// </summary>
    /// <param name="s"></param>
    public void addState(State s)
    {
        statesList.Add(s);
    }

    /// <summary>
    /// Update each component of the state machine
    /// </summary>
    /// <returns></returns>
    public List<Action> UpdateStateMachine()
    {
        triggeredTransition = null;

        // Check through each transition and store the first one that triggers
        foreach (Transition transit in currentState.getTransitions())
        {
            if (transit.isTriggered(controlUnit))
            {
                triggeredTransition = transit;
                break;
            }
        }

        // Check if we have a transition to fire
        if (triggeredTransition != null)
        {
            // Find the target state
            targetState = triggeredTransition.getTargetState();

            // Add the exit action of the old state, the transition action and the entry for the new state
            actions = currentState.getExitAction();
            foreach(Action a in triggeredTransition.getAction())
            {
                actions.Add(a);
            }
            foreach (Action a in targetState.getEntryAction())
            {
                actions.Add(a);
            }

            // Complete the transition and return action list
            currentState = targetState;
            return actions;
        }
        // Otherwise just return the current state's actions
        else
        {
            return currentState.getAction();
        }
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public StateMachine()
    {

    }


}
