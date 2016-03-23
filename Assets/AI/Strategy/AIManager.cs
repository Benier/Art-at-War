//using UnityEngine;
//using System.Collections.Generic;
//using System.Collections;

//public class AIManager : MonoBehaviour
//{
//    [SerializeField]
//    public int health = 5;

//    List<Action> actionList;

//    StateMachine a_FSM;

//    State s_AggrSpread;
//    State s_DefCollapse;

//    Transition t_AggrDef;
//    Transition t_DefAggr;

//    List<Action> aggrTransList;
//    List<Action> defTransList;

//    AggressiveCondition c_Aggr;
//    DefensiveCondition c_Def;


//    // Use this for initialization
//    void Start()
//    {
//        InitAllState();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        actionList = a_FSM.UpdateStateMachine();

//        foreach (Action a in actionList)
//        {
//        //    a.Execute();
//        }
//    }

//    void InitAllState()
//    {
//        s_AggrSpread = new State();
//        s_DefCollapse = new State();

//        aggrTransList = new List<Action>();
//        defTransList = new List<Action>();

//        c_Aggr = new AggressiveCondition();
//        c_Def = new DefensiveCondition();

//        aggrTransList.Add(new PrintAction("Starting Aggressive Spread"));
//        defTransList.Add(new PrintAction("Starting Defensive Collapse"));

//        t_AggrDef = new Transition(s_DefCollapse, defTransList, c_Def);
//        t_DefAggr = new Transition(s_AggrSpread, aggrTransList, c_Aggr);

//        s_AggrSpread.addAction(new AggressiveAction());
//        s_AggrSpread.addEntryAction(new PrintAction("Begin Aggressive Spread"));
//        s_AggrSpread.addExitAction(new PrintAction("End Aggressive Spread"));
//        s_AggrSpread.addTransition(t_AggrDef);

//        s_DefCollapse.addAction(new DefensiveAction());
//        s_DefCollapse.addEntryAction(new PrintAction("Begin Defensive Collapse"));
//        s_DefCollapse.addExitAction(new PrintAction("End Defensive Collapse"));
//        s_DefCollapse.addTransition(t_DefAggr);

//        a_FSM = new StateMachine(s_DefCollapse);
//        a_FSM.addState(s_AggrSpread);
//    }
//}
