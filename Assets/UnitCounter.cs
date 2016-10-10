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
    public string playerFaction;
    public int numEnemy;
    public int numQEnemy;
    public int numPlayer;
    public Text numEnemyText;
	// Use this for initialization
	void Start ()
    {
        pencilSelected = false;
        charcoalSelected = false;
        oilSelected = false;
        waterSelected = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void SelectPencil()
    {
        pencilSelected = true;
        charcoalSelected = false;
        oilSelected = false;
        waterSelected = false;

        playerFaction = "PencilUnitPrefab";
    }

    public void SelectCharcoal()
    {
        pencilSelected = false;
        charcoalSelected = true;
        oilSelected = false;
        waterSelected = false;
    }

    public void SelectOil()
    {
        pencilSelected = false;
        charcoalSelected = false;
        oilSelected = true;
        waterSelected = false;
    }

    public void SelectWater()
    {
        pencilSelected = false;
        charcoalSelected = false;
        oilSelected = false;
        waterSelected = true;
    }
}
