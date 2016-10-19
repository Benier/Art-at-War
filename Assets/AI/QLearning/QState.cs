using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QState
{
    #region nontextbook implementation
    public string statename;
    public List<Action> actions;

    public void AddAction(Action action)
    {
        actions.Add(action);
    }

    #endregion

    public QState(string name)
    {
        statename = name;
        actions = new List<Action>();
    }
}