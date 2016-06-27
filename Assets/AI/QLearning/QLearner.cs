using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class QLearner
{
    QValueStore store;
    QProblem problem;
    int iterations;
    float alpha;
    float gamma;
    float rho;
    float nu;
    QState state;
    List<Action> actions;
    Action action;
    Unit unit;
    TextureGenerator texGen;

    public QLearner(Unit u, TextureGenerator tg)
    {
        InitializeQValues();
        problem = new QProblem();
        unit = u;
        texGen = tg;
        state = problem.GetRandomState();
        store = GameObject.Find("QValueStore").GetComponent<QValueStore>();
        rho = 3;
        nu = 0;
        alpha = 1;
        gamma = 0.2f;
    }
    	
	public void UpdateQ ()
    {
        StateRewardPair srp;
        QState newState;
        float qVal;
        float maxQ;
        float reward;
        float randnu = Random.Range(0.0f, 10.0f);
	    if(randnu < nu)
        {
            state = problem.GetRandomState();
        }
        Debug.Log(state.statename);
        actions = problem.GetAvailableActions(state);

        float randrho = Random.Range(0.0f, 10.0f);
        if(randrho < rho)
        {
            int randInd = Random.Range(0, actions.Count);
            action = actions[randInd];
        }
        else
        {
            action = store.GetBestAction(state);
        }
       
        srp = problem.takeAction(state, action, unit, texGen);
        reward = srp.reward;
        newState = srp.state;

        qVal = store.GetQValue(new StateActionPair(state, action));

        maxQ = store.GetQValue(new StateActionPair(newState, store.GetBestAction(newState)));

        qVal = (1 - alpha) * qVal + alpha * (reward + gamma * maxQ);

        store.storeQValue(state, action, qVal);
        //Debug.Log("Q: " + qVal + ", Action: " + action.GetType() + ", Best Action: " + store.GetBestAction(state).GetType());

        state = newState;
	}

    public void SaveQToFile()
    {
        string savePath = @"QValues.txt";
        List<string> output = new List<string>();
        List<StateActionPair> sapList = store.GetAllStateActionPairs();

        for(int i = 0; i < sapList.Count; i++)
        {
            string outLine = sapList[i].state.statename + "," + sapList[i].action.GetName() + "," + sapList[i].qVal.ToString();
            output.Add(outLine);
        }
        output.Add("<<<<<END>>>>>");
        string[] lines = output.ToArray();

        System.IO.File.WriteAllLines(savePath, lines);
    }

    private string[] LoadQFromFile()
    {
        string savePath = @"QValues.txt";
        string[] lines = System.IO.File.ReadAllLines(savePath);

        return lines;
    }

    private void InitializeQValues()
    {
        string[] textLoadInput = LoadQFromFile();
        for(int i = 0; i < textLoadInput.Length; i++)
        {
            string curLine = textLoadInput[i];
            char[] delimiterChars = { ',' };
            string[] words = curLine.Split(delimiterChars);
            if(words[0] == "Middle")
            {
                if(words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    int x = 0;
                }
            }
        }
    }
}
