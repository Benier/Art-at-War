using UnityEngine;
using System.Collections;
//This class is non-textbook implementation
//public class QActionResult : MonoBehaviour
//{

//    public string StateName { get; internal set; }
//    public string PrevStateName { get; internal set; }
//    public double QValue { get; internal set; } // Q value is stored here        
//    public double Probability { get; internal set; }
//    public double Reward { get; internal set; }

//    public double QEstimated
//    {
//        get { return QValue * Probability; }
//    }

//    public QActionResult(QAction action, string stateNameNext = null,
//        double probability = 1, double reward = 0)
//    {
//        PrevStateName = action.CurrentState;
//        StateName = stateNameNext;
//        Probability = probability;
//        Reward = reward;
//    }

//    //public override string ToString()
//    //{
//    //    return string.Format("State {0}, Prob. {1}, Reward {2}, PrevState {3}, QE {4}",
//    //        StateName, Probability.Pretty(), Reward, PrevStateName, QEstimated.Pretty());
//    //}
//}
