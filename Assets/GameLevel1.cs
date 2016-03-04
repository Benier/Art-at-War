using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevel1 : MonoBehaviour {
    [SerializeField]
    GameObject mapGenerator;
    public static Dictionary<Coordinate, GameObject> map;
    List<GameObject> playerUnits = new List<GameObject>();
    List<GameObject> enemyUnits = new List<GameObject>();
    static int playerRangedUnitCount = 2;
    static int playerMeleeUnitCount = 0;
    static int enemyRangedUnitCount = 2;
    static int enemyMeleeUnitCount = 0;
    public int curUnitInd;
    bool playerTurn;
    int totalPlayerAP;
    int totalEnemyAP;

    MapGenerator mapGen;
    

    void Awake()
    {

    }
    // Use this for initialization
    void Start ()
    {
        mapGen = mapGenerator.GetComponent<MapGenerator>();
        map = mapGen.GenerateMap();
        SpawnUnits();
        curUnitInd = 0;
        EnableUnit(playerUnits, curUnitInd);
        playerTurn = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        for(int i = 0; i < playerUnits.Count; i++)
        {
            totalPlayerAP += playerUnits[i].GetComponent<Unit>().AP;
        }
        for (int i = 0; i < enemyUnits.Count; i++)
        {
            totalEnemyAP += enemyUnits[i].GetComponent<Unit>().AP;
        }

        if (playerTurn)
        {
            if (playerUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
            {
                if (!SelectNextUnit())
                {
                    Debug.Log("Turn Ended");
                    curUnitInd = 0;
                    EnableUnit(enemyUnits, curUnitInd);
                    playerTurn = false;
                    EnableMove();
                    ResetUnitsAP(playerTurn);
                }
            }
        }
        else
        {
            if (enemyUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
            {
                if (!SelectNextUnit())
                {
                    Debug.Log("Turn Ended");
                    curUnitInd = 0;
                    EnableUnit(playerUnits, curUnitInd);
                    playerTurn = true;
                    EnableMove();
                    ResetUnitsAP(playerTurn);
                }
            }
        }

        totalPlayerAP = 0;
        totalEnemyAP = 0;
    }

    void SpawnUnits()
    {
        for (int num = 0; num < playerRangedUnitCount; num++)
        {
            playerUnits.Add(SpawnRangedPlayerUnit());
        }
        for (int num = 0; num < playerMeleeUnitCount; num++)
        {
            playerUnits.Add(SpawnMeleePlayerUnit());
        }

        for (int num = 0; num < enemyRangedUnitCount; num++)
        {
            enemyUnits.Add(SpawnRangedEnemyUnit());
        }
        for (int num = 0; num < enemyMeleeUnitCount; num++)
        {
            enemyUnits.Add(SpawnMeleeEnemyUnit());
        }
    }

    GameObject SpawnRangedPlayerUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("PencilUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().SetType(1);
            unitPrefab.transform.position = new Vector3(x, y, z);
            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnRangedPlayerUnit();
        }
        return unitPrefab;
    }

    GameObject SpawnMeleePlayerUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("PencilUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().SetType(0);
            unitPrefab.transform.position = new Vector3(x, y, z);
            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnMeleePlayerUnit();
        }
        return unitPrefab;
    }

    GameObject SpawnRangedEnemyUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("OilBrushUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().SetType(1);
            unitPrefab.transform.position = new Vector3(x, y, z);
            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnRangedEnemyUnit();
        }
        return unitPrefab;
    }

    GameObject SpawnMeleeEnemyUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("OilBrushUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().SetType(0);
            unitPrefab.transform.position = new Vector3(x, y, z);
            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnMeleeEnemyUnit();
        }
        return unitPrefab;
    }

    public bool SelectNextUnit()
    {
        List<GameObject> unitsList;
        int remainingAP;
        if (playerTurn)
        {
            unitsList = playerUnits;
            remainingAP = totalPlayerAP;
        }
        else
        {
            unitsList = enemyUnits;
            remainingAP = totalEnemyAP;
        }
        if (curUnitInd < unitsList.Count -1)
        {
            DisableUnit(unitsList, curUnitInd);
            curUnitInd++;
            EnableUnit(unitsList, curUnitInd);
            EnableMove();
            return true;
        }
        else
        {
            DisableUnit(unitsList, curUnitInd);
            return false;
        }
    }

    public void UISelectNextUnit()
    {
        SelectNextUnit();
    }

    public bool SelectPreviousUnit()
    {
        List<GameObject> unitsList;
        int remainingAP;
        if (playerTurn)
        {
            unitsList = playerUnits;
            remainingAP = totalPlayerAP;
        }
        else
        {
            unitsList = enemyUnits;
            remainingAP = totalEnemyAP;
        }
        if (curUnitInd > 0)
        {
            DisableUnit(unitsList, curUnitInd);
            curUnitInd--;
            EnableUnit(unitsList, curUnitInd);
            EnableMove();
            return true;
        }
        else
        {
            DisableUnit(unitsList, curUnitInd);
            return false;
        }
    }

    public void UISelectPreviousUnit()
    {
        SelectPreviousUnit();
    }

    public void EnableAttack()
    {
        List<GameObject> unitsList;
        if (playerTurn)
        {
            unitsList = playerUnits;
        }
        else
        {
            unitsList = enemyUnits;
        }
        unitsList[curUnitInd].GetComponent<Unit>().ability = Unit.Ability.Attack;
        //playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = true;
        //playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = false;
        //playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = false;
    }

    public void EnableTar()
    {
        List<GameObject> unitsList;
        if (playerTurn)
        {
            unitsList = playerUnits;
        }
        else
        {
            unitsList = enemyUnits;
        }
        unitsList[curUnitInd].GetComponent<Unit>().ability = Unit.Ability.Tar;
        //playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = false;
        //playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = true;
        //playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = false;
    }

    public void EnableMove()
    {
        List<GameObject> unitsList;
        if (playerTurn)
        {
            unitsList = playerUnits;
        }
        else
        {
            unitsList = enemyUnits;
        }
        unitsList[curUnitInd].GetComponent<Unit>().ability = Unit.Ability.Move;
        //playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = false;
        //playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = false;
        //playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = true;
    }

    void EnableUnit(List<GameObject> units, int index)
    {
        units[index].GetComponent<Unit>().active = true;
    }

    void DisableUnit(List<GameObject> units, int index)
    {
        units[index].GetComponent<Unit>().active = false;
    }

    void ResetUnitsAP(bool isPlayer)
    {
        List<GameObject> unitsList;
        if (isPlayer)
        {
            unitsList = playerUnits;
        }
        else
        {
            unitsList = enemyUnits;
        }
        for(int i = 0; i < unitsList.Count; i++)
        {
            unitsList[i].GetComponent<Unit>().ResetAP();
        }
    }
}
