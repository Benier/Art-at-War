using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LearningProblem
{
    int stateCount;
    int actionsPerState;
    List<LearningProblemState> states;
    List<LearningProblemAction> actions;
    int GetStateCount()
    {
        return stateCount;
    }
    int GetActionCount()
    {
        return actionsPerState;
    }
    LearningProblemState GetState(int index)
    {
        return states[index];
    }
    LearningProblemState GetRandomState()
    {
        return states[Random.Range(0, states.Count - 1)];
    }
    LearningProblemState GetInitialState()
    {
        return GetRandomState();
    }
    LearningProblemAction GetActions(LearningProblemState state);
    LearningProblemActionResult GetResult(LearningProblemState state, LearningProblemAction action);
}
