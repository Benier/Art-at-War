using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Agent : MonoBehaviour {

    List<Action> actionList;

    StateMachine a_FSM;

    State s_Wander;
    State s_Attack;

    Transition t_AttackWander;
    Transition t_WanderAttack;

    List<Action> attackTransList;
    List<Action> wanderTransList;

    AttackCondition c_Attack;
    WanderCondition c_Wander;


	// Use this for initialization
	void Start () {
        InitAllState();
	}
	
	// Update is called once per frame
	void Update ()
    {
        actionList = a_FSM.UpdateStateMachine();

        foreach (Action a in actionList)
        {
            a.Execute();
        }
	}

    void InitAllState()
    {
        s_Attack = new State();
        s_Wander = new State();

        attackTransList = new List<Action>();
        wanderTransList = new List<Action>();

        c_Attack = new AttackCondition();
        c_Wander = new WanderCondition();

        attackTransList.Add(new PrintAction("Starting Attack"));
        wanderTransList.Add(new PrintAction("Starting Wander"));

        t_AttackWander = new Transition(s_Wander, wanderTransList, c_Wander);
        t_WanderAttack = new Transition(s_Attack, attackTransList, c_Attack);

        s_Attack.addAction(new AttackAction());
        s_Attack.addEntryAction(new PrintAction("Begin Attack"));
        s_Attack.addExitAction(new PrintAction("End Attack"));
        s_Attack.addTransition(t_AttackWander);

        s_Wander.addAction(new WanderAction());
        s_Wander.addEntryAction(new PrintAction("Begin Wander"));
        s_Wander.addExitAction(new PrintAction("End Wander"));
        s_Wander.addTransition(t_WanderAttack);

        a_FSM = new StateMachine(s_Wander);
        a_FSM.addState(s_Attack);
    }
}
