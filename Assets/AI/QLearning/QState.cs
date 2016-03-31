using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QState
{
    #region nontextbook implementation
    public string statename;
    public List<QAction> actions;

    public void AddAction(QAction action)
    {
        actions.Add(action);
    }

    //public QState(string stateName, QLearning q)
    //{
    //    q.StateLookup.Add(stateName, this);
    //    StateName = stateName;
    //    actions = new List<QAction>();
    //}

    //public override string ToString()
    //{
    //    return string.Format("StateName {0}", StateName);
    //}
    #endregion

    public QState(string name)
    {
        statename = name;
        actions = new List<QAction>();
    }
}