﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QLearner : MonoBehaviour
{
    QValueStore store;
    QProblem problem;
    int iterations;
    float alpha;
    float gamma;
    float rho;
    float nu;
    QState state;
    List<QAction> actions;
    QAction action;
    
	// Use this for initialization
	void Start ()
    {
        state = problem.GetRandomState();
        store = GameObject.Find("QValueStore").GetComponent<QValueStore>();
	}
	
	// Update is called once per frame
	void UpdateQ ()
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

        actions = problem.GetAvailableActions(state);

        float randrho = Random.Range(0.0f, 10.0f);
        if(randrho < rho)
        {
            int randInd = Random.Range(0, actions.Count - 1);
            action = actions[randInd];
        }
        else
        {
            action = store.GetBestAction(state);
        }

        srp = problem.takeAction(state, action);
        reward = srp.reward;
        newState = srp.state;

        qVal = store.GetQValue(new StateActionPair(state, action));

        maxQ = store.GetQValue(new StateActionPair(newState, store.GetBestAction(newState)));

        qVal = (1 - alpha) * qVal + alpha * (reward + gamma * maxQ);

        store.storeQValue(state, action, qVal);

        state = newState;
	}
}
