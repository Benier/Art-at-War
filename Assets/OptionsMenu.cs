using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    OptionsQLearner invisQLearner;
    GameObject optionsHolder;
	// Use this for initialization
	void Start ()
    {
        invisQLearner = new OptionsQLearner();
        optionsHolder = GameObject.Find("OptionsHolder");
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void BackToMainMenu()
    {
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
