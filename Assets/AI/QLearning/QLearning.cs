using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

public class QLearning
{
    public List<QState> States;
    public Dictionary<string, QState> StateLookup;

    public double Alpha;
    public double Gamma;

    public HashSet<string> EndStates;
    public int MaxExploreStepsWithinOneEpisode; //avoid infinite loop
    public bool ShowWarning; // show runtime warnings regarding q-learning
    public int Episodes;

    public QLearning()
    {
        States = new List<QState>();
        StateLookup = new Dictionary<string, QState>();
        EndStates = new HashSet<string>();

        // Default when not set
        MaxExploreStepsWithinOneEpisode = 1000;
        Episodes = 1000;
        Alpha = 0.1;
        Gamma = 0.9;
        ShowWarning = true;
    }

    public void AddState(QState state)
    {
        States.Add(state);
    }

    public void RunTraining()
    {
        QMethod.Validate(this);

        /*       
        For each episode: Select random initial state 
        Do while not reach goal state
            Select one among all possible actions for the current state 
            Using this possible action, consider to go to the next state 
            Get maximum Q value of this next state based on all possible actions                
            Set the next state as the current state
        */

        // For each episode
        var rand = new Random();
        long maxloopEventCount = 0;

        // Train episodes
        for (long i = 0; i < Episodes; i++)
        {
            long maxloop = 0;
            // Select random initial state          
            int stateIndex = rand.Next(States.Count);
            QState state = States[stateIndex];
            QAction action = null;
            do
            {
                if (++maxloop > MaxExploreStepsWithinOneEpisode)
                {
                    if (ShowWarning)
                    {
                        string msg = string.Format(
                        "{0} !! MAXLOOP state: {1} action: {2}, {3} endstate is to difficult to reach?",
                        ++maxloopEventCount, state, action, "maybe your path setup is wrong or the ");
                        QMethod.Log(msg);
                    }

                    break;
                }

                // no actions, skip this state
                if (state.Actions.Count == 0)
                    break;

                // Selection strategy is random based on probability
                int index = rand.Next(state.Actions.Count);
                action = state.Actions[index];

                // Using this possible action, consider to go to the next state
                // Pick random Action outcome
                QActionResult nextStateResult = action.PickActionByProbability();
                string nextStateName = nextStateResult.StateName;

                double q = nextStateResult.QEstimated;
                double r = nextStateResult.Reward;
                double maxQ = MaxQ(nextStateName);

                // Q(s,a)= Q(s,a) + alpha * (R(s,a) + gamma * Max(next state, all actions) - Q(s,a))
                double value = q + Alpha * (r + Gamma * maxQ - q); // q-learning                  
                nextStateResult.QValue = value; // update

                // is end state go to next episode
                if (EndStates.Contains(nextStateResult.StateName))
                    break;

                // Set the next state as the current state                    
                state = StateLookup[nextStateResult.StateName];

            } while (true);
        }
    }


    double MaxQ(string stateName)
    {
        const double defaultValue = 0;

        if (!StateLookup.ContainsKey(stateName))
            return defaultValue;

        QState state = StateLookup[stateName];
        var actionsFromState = state.Actions;
        double? maxValue = null;
        foreach (var nextState in actionsFromState)
        {
            foreach (var actionResult in nextState.ActionsResult)
            {
                double value = actionResult.QEstimated;
                if (value > maxValue || !maxValue.HasValue)
                    maxValue = value;
            }
        }

        // no update
        if (!maxValue.HasValue && ShowWarning)
            QMethod.Log(string.Format("Warning: No MaxQ value for stateName {0}",
                stateName));

        return maxValue.HasValue ? maxValue.Value : defaultValue;
    }

    public void PrintQLearningStructure()
    {
        Console.WriteLine("** Q-Learning structure **");
        foreach (QState state in States)
        {
            Console.WriteLine("State {0}", state.StateName);
            foreach (QAction action in state.Actions)
            {
                Console.WriteLine("  Action " + action.ActionName);
                Console.Write(action.GetActionResults());
            }
        }
        Console.WriteLine();
    }

    public void ShowPolicy()
    {
        Console.WriteLine("** Show Policy **");
        foreach (QState state in States)
        {
            double max = Double.MinValue;
            string actionName = "nothing";
            foreach (QAction action in state.Actions)
            {
                foreach (QActionResult actionResult in action.ActionsResult)
                {
                    if (actionResult.QEstimated > max)
                    {
                        max = actionResult.QEstimated;
                        actionName = action.ActionName.ToString();
                    }
                }
            }

            //Console.WriteLine(string.Format("From state {0} do action {1}, max QEstimated is {2}",
            //    state.StateName, actionName, max.Pretty()));
        }
    }
}






