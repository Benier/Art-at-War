using System.Collections.Generic;
using System.Collections;

public class Agent
{
    public int health = 5;

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

    TextureGenerator texGen;

    public Unit controlUnit;

    public Agent(Unit u, TextureGenerator tg)
    {
        controlUnit = u;
        texGen = tg;
        InitAllState();
    }
	
	// Update is called once per frame
	public void Update ()
    {
        actionList = a_FSM.UpdateStateMachine();

        foreach (Action a in actionList)
        {
            a.Execute(controlUnit);
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


        t_AttackWander = new Transition(s_Wander, wanderTransList, c_Wander, texGen);
        t_WanderAttack = new Transition(s_Attack, attackTransList, c_Attack, texGen);

        s_Attack.addAction(new AttackAction());

        s_Attack.addTransition(t_AttackWander);

        s_Wander.addAction(new WanderAction());

        s_Wander.addTransition(t_WanderAttack);

        a_FSM = new StateMachine(s_Wander, controlUnit);
        a_FSM.addState(s_Attack);
    }
}
