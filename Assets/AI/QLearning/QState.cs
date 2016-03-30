using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QState
{
    #region nontextbook implementation
    public string StateName { get; private set; }
    public List<QAction> Actions { get; private set; }

    public void AddAction(QAction action)
    {
        Actions.Add(action);
    }

    public QState(string stateName, QLearning q)
    {
        q.StateLookup.Add(stateName, this);
        StateName = stateName;
        Actions = new List<QAction>();
    }

    public override string ToString()
    {
        return string.Format("StateName {0}", StateName);
    }
    #endregion

}