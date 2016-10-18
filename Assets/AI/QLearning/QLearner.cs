using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class QLearner
{
    List<QValueStore> stores;
    GameLevel1 gameLevel;
    QValueStore store;
    QProblem problem;
    int iterations;
    int maxIterations;
    int stepsback;
    int losingStreak;
    int maxLosingStreak;
    float alpha; //learning rate
    float gamma; //reinforcement value
    float rho; //random action threshold
    float nu; //random state threshold
    QState state;
    List<Action> actions;
    Action action;
    Unit unit;
    TextureGenerator texGen;

    public QLearner(Unit u, TextureGenerator tg)
    {
        stores = new List<QValueStore>();
        gameLevel = GameObject.Find("GameLvl1").GetComponent<GameLevel1>();
        problem = new QProblem();
        iterations = 0;
        maxIterations = gameLevel.numQValStores;
        losingStreak = 0;
        maxLosingStreak = 3;
        stepsback = 4;
        unit = u;
        texGen = tg;
        state = problem.GetRandomState();

        for (int num = 0; num < maxIterations; num++)
        {
            stores.Add(gameLevel.qValStores[num]);
        }

        InitializeQValues();
        if (iterations != 0)
        {
            int prevIterations = iterations - 1;
            if (prevIterations < stores.Count - 1)
            {
                stores[prevIterations].copyToStore(stores[iterations]);
                //stores[iterations + 1] = stores[iterations];
                store = stores[iterations];
                //stores[iterations + 1].copyToStore(store);
            }
            else
            {
                //stores[iterations] = stores[iterations];
                store = stores[prevIterations];
                //stores[iterations].copyToStore(stores[iterations]);
                stores[prevIterations].copyToStore(store);
            }
        }
        else
        {
            store = stores[iterations];
            stores[iterations].copyToStore(store);
        }

        rho = 10;
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
        //Debug.Log(state.statename);
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

    public void SaveQToFileAppend()
    {
        string savePath = @"QValues.txt";
        List<string> output = new List<string>();
        List<StateActionPair> sapList = store.GetAllStateActionPairs();

        output.Add("<<<<<LOSINGSTREAK>>>>>" + "," + losingStreak);
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

    public void SaveQToFileOverwrite()
    {
        string savePath = @"QValues.txt";
        List<string> output = new List<string>();
        List<StateActionPair> sapList;

        output.Add("<<<<<LOSINGSTREAK>>>>>" + "," + losingStreak);
        for (int num = 0; num < stores.Count; num++)
        {
            sapList = stores[num].GetAllStateActionPairs();
            for (int i = 0; i < sapList.Count; i++)
            {
                string outLine = sapList[i].state.statename + "," + sapList[i].action.GetName() + "," + ((int)sapList[i].qVal).ToString();
                output.Add(outLine);
            }
            output.Add("<<<<<END>>>>>" + "," + iterations);
        }
        string[] lines = output.ToArray();

        System.IO.File.WriteAllLines(savePath, lines);
    }

    public void IncrementLosingStreak()
    {
        losingStreak++;
        if(losingStreak == maxLosingStreak)
        {
            RevertQ();
            losingStreak = 0;
        }
    }

    public void RevertQ()
    {
        int revertIterations = iterations - stepsback;
        revertIterations = Mathf.Clamp(revertIterations, 0, maxIterations - 1);
        iterations = Mathf.Clamp(iterations, 0, maxIterations - 1);
        stores[iterations].copyToStore(stores[revertIterations]);
        for(int i = revertIterations + 1; i < stores.Count; i++)
        {
            stores[i].zeroOutValues();
        }
    }

    public void ClearAllQ()
    {
        for(int i = 0; i < stores.Count; i++)
        {
            stores[i].zeroOutValues();
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
        bool empty = true;
        string[] textLoadInput = LoadQFromFile();
        for(int i = 0; i < textLoadInput.Length; i++)
        {
            string curLine = textLoadInput[i];
            char[] delimiterChars = { ',' };
            string[] words = curLine.Split(delimiterChars);

            if(words[0] == "<<<<<LOSINGSTREAK>>>>>")
            {
                losingStreak = int.Parse(words[1]);
            }
            
            if (words[0] == "Middle")
            {
                if(words[1] == "WanderNEAction")
                {
                    int QVal;
                    bool validResult = int.TryParse(words[2], out QVal);
                    if (validResult)
                    {
                        QVal = int.Parse(words[2]);
                    }
                    stores[iterations].storeQValue(new QState("Middle"), new WanderNEAction(), QVal);
                    if(QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new WanderNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new WanderSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new WanderSWAction(), QVal); if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new TarNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new TarNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new TarSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new TarSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
            }
            if (words[0] == "NW Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new WanderSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new AttackNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("Middle"), new AttackNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new AttackSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new AttackSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new TarNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new TarNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new TarSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NW Corner"), new TarSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
            }
            if (words[0] == "NE Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new WanderSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new AttackSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new TarNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new TarNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new TarSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("NE Corner"), new TarSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
            }
            if (words[0] == "SW Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new WanderSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new AttackSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new TarNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new TarNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new TarSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SW Corner"), new TarSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
            }
            if (words[0] == "SE Corner")
            {
                if (words[1] == "WanderNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "WanderSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new WanderSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackNEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackNWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackNWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackSEAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "AttackSWAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new AttackSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
                if (words[1] == "TarNEAction")
                {
                    int QVal = int.Parse(words[2]);
                    stores[iterations].storeQValue(new QState("SE Corner"), new TarNEAction(), QVal);
                    if (QVal != 0)                                              
                    {                                                           
                        empty = false;                                          
                    }                                                           
                }                                                               
                if (words[1] == "TarNWAction")                                  
                {                                                               
                    int QVal = int.Parse(words[2]);                              
                    stores[iterations].storeQValue(new QState("SE Corner"), new TarNWAction(), QVal);
                    if (QVal != 0)                                              
                    {                                                           
                        empty = false;                                          
                    }                                                           
                }                                                               
                if (words[1] == "TarSEAction")                                  
                {                                                               
                    int QVal = int.Parse(words[2]);                             
                    stores[iterations].storeQValue(new QState("SE Corner"), new TarSEAction(), QVal);
                    if (QVal != 0)                                              
                    {                                                           
                        empty = false;                                          
                    }                                                           
                }                                                               
                if (words[1] == "TarSWAction")                                  
                {                                                               
                    int QVal = int.Parse(words[2]);                              
                    stores[iterations].storeQValue(new QState("SE Corner"), new TarSWAction(), QVal);
                    if (QVal != 0)
                    {
                        empty = false;
                    }
                }
            }
            if(words[0] == "<<<<<END>>>>>" && iterations < maxIterations && !empty)
            {
                iterations++;
                empty = true;
            }
        }
    }
}
