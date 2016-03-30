using UnityEngine;
using System.Collections;

public class StateActionPair
{
    public QState state;
    public QAction action;
    public float qVal;
    public StateActionPair(QState s, QAction a)
    {
        state = s;
        action = a;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
