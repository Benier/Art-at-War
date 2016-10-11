using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitCounter : MonoBehaviour
{
    public bool pencilSelected;
    public bool charcoalSelected;
    public bool oilSelected;
    public bool waterSelected;
    public string enemyFaction;
    public string qEnemyFaction;
    public string playerFaction;
    public int numEnemy;
    public int numQEnemy;
    public int numPlayer;
    public Text numEnemyText;
    public Text numPlayerText;
    public Toggle qLearningEnemyToggle;
    ArrayList enemyList = new ArrayList();
	// Use this for initialization
	void Start ()
    {
        pencilSelected = false;
        charcoalSelected = false;
        oilSelected = false;
        waterSelected = false;

        numEnemy = 0;
        numQEnemy = int.Parse(numEnemyText.text);
        numPlayer = int.Parse(numPlayerText.text);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void IncrementEnemy()
    {
        if(qLearningEnemyToggle.isOn)
        {
            numQEnemy++;
            numEnemyText.text = numQEnemy.ToString();
            numEnemy = 0;
        }
        else
        {
            numEnemy++;
            numEnemyText.text = numEnemy.ToString();
            numQEnemy = 0;
        }        
    }

    public void DecrementEnemy()
    {
        if (qLearningEnemyToggle.isOn)
        {
            numQEnemy--;
            numEnemyText.text = numQEnemy.ToString();
            numEnemy = 0;
        }
        else
        {
            numEnemy--;
            numEnemyText.text = numEnemy.ToString();
            numQEnemy = 0;
        }
    }

    public void IncrementPlayer()
    {
        numPlayer++;
        numPlayerText.text = numPlayer.ToString();
    }

    public void DecrementPlayer()
    {
        numPlayer++;
        numPlayerText.text = numPlayer.ToString();
    }

    public void SelectPencil()
    {
        playerFaction = "PencilUnitPrefab";
        enemyList.Add("CharcoalUnitPrefab");
        enemyList.Add("OilBrushUnitPrefab");
        enemyList.Add("WaterBrushUnitPrefab");

        int randomIndex = Random.Range(0, enemyList.Count);
        enemyFaction = (string)enemyList[randomIndex];
        qEnemyFaction = (string)enemyList[randomIndex];
    }

    public void SelectCharcoal()
    {
        playerFaction = "CharcoalUnitPrefab";
        enemyList.Add("PencilUnitPrefab");
        enemyList.Add("OilBrushUnitPrefab");
        enemyList.Add("WaterBrushUnitPrefab");

        int randomIndex = Random.Range(0, enemyList.Count);
        enemyFaction = (string)enemyList[randomIndex];
        qEnemyFaction = (string)enemyList[randomIndex];
    }

    public void SelectOil()
    {
        playerFaction = "OilBrushUnitPrefab";
        enemyList.Add("CharcoalUnitPrefab");
        enemyList.Add("PencilUnitPrefab");
        enemyList.Add("WaterBrushUnitPrefab");

        int randomIndex = Random.Range(0, enemyList.Count);
        enemyFaction = (string)enemyList[randomIndex];
        qEnemyFaction = (string)enemyList[randomIndex];
    }

    public void SelectWater()
    {
        playerFaction = "WaterBrushUnitPrefab";
        enemyList.Add("CharcoalUnitPrefab");
        enemyList.Add("OilBrushUnitPrefab");
        enemyList.Add("PencilUnitPrefab");

        int randomIndex = Random.Range(0, enemyList.Count);
        enemyFaction = (string)enemyList[randomIndex];
        qEnemyFaction = (string)enemyList[randomIndex];
    }
}
