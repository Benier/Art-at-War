using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    OptionsQLearner invisQLearner;
    GameObject optionsHolder;
    OptionsHolder holder;
    public InputField alphaIF;
    public InputField gammaIF;
    public InputField rhoIF;
    public InputField nuIF;
    public InputField maxIterationsIF;
    public InputField stepsbackIF;
    public InputField maxLosingStreakIF;
	// Use this for initialization
	void Start ()
    {
        invisQLearner = new OptionsQLearner();
        optionsHolder = GameObject.Find("OptionsHolder");
        holder = optionsHolder.GetComponent<OptionsHolder>();
        alphaIF.text = holder.alpha.ToString();
        gammaIF.text = holder.gamma.ToString();
        rhoIF.text = holder.rho.ToString();
        nuIF.text = holder.nu.ToString();
        maxIterationsIF.text = holder.maxIterations.ToString();
        stepsbackIF.text = holder.stepsback.ToString();
        maxLosingStreakIF.text = holder.maxLosingStreak.ToString();
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void BackToMainMenu()
    {
        holder.alpha = float.Parse(alphaIF.text);
        holder.gamma = float.Parse(gammaIF.text);
        holder.rho = float.Parse(rhoIF.text);
        holder.nu = float.Parse(nuIF.text);
        holder.maxIterations = (int)float.Parse(maxIterationsIF.text);
        holder.stepsback = (int)float.Parse(stepsbackIF.text);
        holder.maxLosingStreak = (int)float.Parse(maxLosingStreakIF.text);
        DontDestroyOnLoad(optionsHolder);
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetAllQ()
    {
        invisQLearner.ClearAllQ();
    }

    GameObject SpawnInvisibleQRangedEnemyUnit()
    {
        GameObject unitPrefab;
        float x = -500;
        float y = -500;
        float z = -500;

        unitPrefab = Instantiate(Resources.Load("QPencilUnitPrefab", typeof(GameObject))) as GameObject;
        unitPrefab.GetComponent<Unit>().type = Unit.Type.Oil;
        unitPrefab.transform.position = new Vector3(x, y, z);

        return unitPrefab;
    }
}
