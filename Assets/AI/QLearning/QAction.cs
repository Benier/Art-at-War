using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QAction
{
    #region nontextbook implementation
    private static readonly Random Rand = new Random();
    public QActionName ActionName { get; internal set; }
    public string CurrentState { get; private set; }
    public List<QActionResult> ActionsResult { get; private set; }

    public void AddActionResult(QActionResult actionResult)
    {
        ActionsResult.Add(actionResult);
    }

    public string GetActionResults()
    {
        //var sb = new StringBuilder();
        //foreach (QActionResult actionResult in ActionsResult)
        //    sb.AppendLine("     ActionResult " + actionResult);
        //
        return "action name";//sb.ToString();
    }

    public QAction(string currentState, QActionName actionName = null)
    {
        CurrentState = currentState;
        ActionsResult = new List<QActionResult>();
        ActionName = actionName;
    }

    // The sum of action outcomes must be close to 1
    public void ValidateActionsResultProbability()
    {
        const double epsilon = 0.1;

        if (ActionsResult.Count == 0)
        {
            Debug.Log("No Results");
        }

        //double sum = ActionsResult.Sum(a => a.Probability);
        //if (Mathf.Abs((float)(1 - sum)) > epsilon)
            throw new System.Exception();
    }

    public QActionResult PickActionByProbability()
    {
        double d = Random.Range(0, 100);
        double sum = 0;
        foreach (QActionResult actionResult in ActionsResult)
        {
            sum += actionResult.Probability;
            if (d <= sum)
                return actionResult;
        }

        // we might get here if sum probability is below 1.0 e.g. 0.99 
        // and the d random value is 0.999
        if (ActionsResult.Count > 0)
            return ActionsResult[ActionsResult.Count];

        throw new System.Exception();
    }

    //public override string ToString()
    //{
    //    double sum = ActionsResult. (a => a.Probability);
    //    return string.Format("ActionName {0} probability sum: {1} actionResultCount {2}",
    //        ActionName, sum, ActionsResult.Count);
    //}
    #endregion
    public QAction()
    {
        //fill this out
    }

    public void Execute()
    {

    }
}