using UnityEngine;
using System.Collections;

public class OptionsHolder : MonoBehaviour {
    public float alpha; //learning rate
    public float gamma; //reinforcement value
    public float rho; //random action threshold
    public float nu; //random state threshold
    public int maxIterations;
    public int stepsback;
    public int maxLosingStreak;
    public bool initialStart;

    // Use this for initialization
    void Start ()
    {
        initialStart = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void InitializeValues()
    {
        rho = 10;
        nu = 0;
        alpha = 1;
        gamma = 0.2f;
        maxIterations = 20;
        maxLosingStreak = 3;
        stepsback = 4;
    }
}
