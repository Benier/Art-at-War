using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LearningProblemAction
{
    int index;
    Action action;
    List<LearningProblemAction> actionList;

    //Returns number of actions in list
    int GetCount()
    {
        return actionList.Count;
    }

    LearningProblemAction GetAtPositionInList(int position)
    {
        return actionList[position];
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
