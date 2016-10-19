using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class OptionsQLearner
{
    List<QValueStore> stores;
    OptionsHolder optionsHolder;
    GameLevel1 gameLevel;
    QValueStore store;
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

    public OptionsQLearner()
    {
        stores = new List<QValueStore>();
        optionsHolder = GameObject.Find("OptionsHolder").GetComponent<OptionsHolder>();
        for(int num = 0; num < optionsHolder.maxIterations; num++)
        {
            QValueStore temp = new QValueStore();
            stores.Add(temp);
        }
    }
    	
	public void UpdateQ ()
    {

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


    public void ClearAllQ()
    {
        for(int i = 0; i < stores.Count; i++)
        {
            stores[i].zeroOutValues();
        }
    }
}
