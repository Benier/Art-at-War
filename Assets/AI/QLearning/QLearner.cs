using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class QLearner
{
    List<QValueStore> stores;
    QValueStore store;
    QProblem problem;
    int iterations;
    int maxIterations;
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
        stores = new List<QValueStore>();
        problem = new QProblem();
        maxIterations = 5;
        unit = u;
        texGen = tg;
        state = problem.GetRandomState();
        stores.Add(GameObject.Find("QValueStore1").GetComponent<QValueStore>());
        stores.Add(GameObject.Find("QValueStore2").GetComponent<QValueStore>());
        stores.Add(GameObject.Find("QValueStore3").GetComponent<QValueStore>());
        stores.Add(GameObject.Find("QValueStore4").GetComponent<QValueStore>());
        stores.Add(GameObject.Find("QValueStore5").GetComponent<QValueStore>());

        InitializeQValues();
        store = stores[iterations - 1];
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

        for (int i = 0; i < sapList.Count; i++)
        {
            string outLine = sapList[i].state.statename + "," + sapList[i].action.GetName() + "," + sapList[i].qVal.ToString();
            output.Add(outLine);
        }
        output.Add("<<<<<END>>>>>" + "," + iterations);
        string[] lines = output.ToArray();

        for (int i = 0; i < lines.Length; i++)
        {
            System.IO.File.AppendAllText(savePath, lines[i] + "\r\n");
        }
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
                    stores[iterations].storeQValue(new QState("Middle"), new WanderNEAction(), QVal);
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new WanderNWAction(), QVal);
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new WanderSEAction(), QVal);
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new WanderSWAction(), QVal);
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackNEAction(), QVal);
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackNWAction(), QVal);
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackSEAction(), QVal);
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackSWAction(), QVal);
                }
            }
            if (words[0] == "NW Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderNEAction(), QVal);
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderNWAction(), QVal);
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderSEAction(), QVal);
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderSWAction(), QVal);
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new AttackNEAction(), QVal);
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackNWAction(), QVal);
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new AttackSEAction(), QVal);
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new AttackSWAction(), QVal);
                }
            }
            if (words[0] == "NE Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderNEAction(), QVal);
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderNWAction(), QVal);
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderSEAction(), QVal);
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderSWAction(), QVal);
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackNEAction(), QVal);
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackNWAction(), QVal);
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackSEAction(), QVal);
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackSWAction(), QVal);
                }
            }
            if (words[0] == "SW Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderNEAction(), QVal);
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderNWAction(), QVal);
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderSEAction(), QVal);
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderSWAction(), QVal);
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackNEAction(), QVal);
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackNWAction(), QVal);
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackSEAction(), QVal);
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackSWAction(), QVal);
                }
            }
            if (words[0] == "SE Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderNEAction(), QVal);
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderNWAction(), QVal);
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderSEAction(), QVal);
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderSWAction(), QVal);
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackNEAction(), QVal);
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackNWAction(), QVal);
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackSEAction(), QVal);
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackSWAction(), QVal);
                }
            }
            if(words[0] == "<<<<<END>>>>>" && iterations < maxIterations - 1)
            {
                iterations++;
            }
        }
    }
}
