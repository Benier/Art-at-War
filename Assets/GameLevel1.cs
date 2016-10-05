﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameLevel1 : MonoBehaviour {
    [SerializeField]
    GameObject mapGenerator;
    public GameObject loadingActTexture;
    TextureGenerator texGenerator;
    public Canvas endGameCanvas;
    public static Dictionary<Coordinate, GameObject> map;
    List<GameObject> playerUnits = new List<GameObject>();
    List<GameObject> enemyUnits = new List<GameObject>();
    List<GameObject> qEnemyUnits = new List<GameObject>();
    List<Agent> agents = new List<Agent>();
    List<QLearner> qagents = new List<QLearner>();
    public List<QValueStore> qValStores = new List<QValueStore>();
    static int playerRangedUnitCount = 1;
    static int playerMeleeUnitCount = 0;
    static int enemyRangedUnitCount = 3;
    static int enemyMeleeUnitCount = 0;
    static int qEnemyRangedUnitCount = 1;
    static int qEnemyMeleeUnitCount = 0;
    public int curUnitInd;
    public int numQValStores = 5;
    int playerTurn; // 1 = player, 2 = enemy, 3 = q enemy
    int totalPlayerAP;
    int totalEnemyAP;
    int totalQEnemyAP;
    int numTurns = 1;
    public int playerPoints;
    public int enemyPoints;
    public int qEnemyPoints;
    int winner; //0 = none, 1 = player, 2 = enemy, 3 = q enemy
    bool gameEnd;

    MapGenerator mapGen;

    void Awake()
    {
        GameObject qvs;
        for (int num = 0; num < numQValStores; num++)
        {
            qvs = Instantiate(Resources.Load("QValueStorePrefab", typeof(GameObject))) as GameObject;
            qValStores.Add(qvs.GetComponent<QValueStore>());
        }
    }
    // Use this for initialization
    void Start ()
    {
        mapGen = mapGenerator.GetComponent<MapGenerator>();
        texGenerator = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        endGameCanvas = GameObject.Find("EndGameCanvas").GetComponent<Canvas>();
        endGameCanvas.enabled = false;
        map = mapGen.GenerateMap(texGenerator);
        SpawnUnits();
        curUnitInd = 0;
        EnableUnit(playerUnits, curUnitInd);
        playerTurn = 1;
        gameEnd = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (numTurns > 0)
        {
            for (int i = 0; i < playerUnits.Count; i++)
            {
                totalPlayerAP += playerUnits[i].GetComponent<Unit>().AP;
            }
            for (int i = 0; i < enemyUnits.Count; i++)
            {
                totalEnemyAP += enemyUnits[i].GetComponent<Unit>().AP;
            }

            if (playerTurn == 1)
            {
                if (playerUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
                {
                    if (!SelectNextUnit())
                    {
                        //Debug.Log("Turn Ended");
                        curUnitInd = 0;
                        EnableUnit(enemyUnits, curUnitInd);
                        playerTurn = 2;
                        EnableMove();
                        ResetUnitsAP(playerTurn);
                    }
                }
            }
            else if (playerTurn == 2)
            {
                agents[curUnitInd].Update();
                if (enemyUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
                {
                    if (!SelectNextUnit())
                    {
                        //Debug.Log("Turn Ended");
                        curUnitInd = 0;
                        EnableUnit(qEnemyUnits, curUnitInd);
                        playerTurn = 3;
                        EnableMove();
                        ResetUnitsAP(playerTurn);
                        //numTurns--;
                    }
                }
            }
            else if (playerTurn == 3)
            {
                qagents[curUnitInd].UpdateQ();
                if (qEnemyUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
                {
                    if (!SelectNextUnit())
                    {
                        //Debug.Log("Turn Ended");
                        curUnitInd = 0;
                        EnableUnit(playerUnits, curUnitInd);
                        playerTurn = 1;
                        EnableMove();
                        ResetUnitsAP(playerTurn);
                        numTurns--;
                    }
                }
            }

            totalPlayerAP = 0;
            totalEnemyAP = 0;
        }
        else
        {
            if(playerPoints > enemyPoints)
            {
                winner = 1;
            }
            else if(playerPoints < enemyPoints)
            {
                winner = 2;
            }
            else
            {
                winner = 0;
            }

            gameEnd = true;
        }
        //for repetitive value growth against statemachine, q agents play for player
        //if(gameEnd)
        //{
        //    UpdateQValues();
        //    if (winner == 1)
        //    {
        //        RevertQValues();
        //    }
        //    else
        //    {

        //        UpdateQValues();
        //    }
        //}
        
    }

    void LateUpdate()
    {
        if(playerTurn != 1) //if it's not player's turn
        {
            loadingActTexture.SetActive(true);
        }
        else
        {
            loadingActTexture.SetActive(false);
        }
        if(gameEnd)
        {
            endGameCanvas.enabled = true;
        }
    }

    public void CloseApplication()
    {
        //for repetitive value growth against statemachine, q agents play for player
        if (gameEnd)
        {
            gameEnd = false;
            if (winner != 1)
            {
                RecordLoss();
            }
            else
            {

                UpdateQValues();
            }            
        }
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        EndGameSaveQ();
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        EndGameSaveQ();
        SceneManager.LoadScene("GameLvl1");
    }

    public void EndGameSaveQ()
    {
        //for repetitive value growth against statemachine, q agents play for player
        if (gameEnd)
        {
            gameEnd = false;
            if (winner != 1) //currently set as revert when player wins for testing
            {
                RecordLoss();
            }
            else
            {
                UpdateQValues();
            }
        }
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
            GameObject tempUnit = SpawnRangedEnemyUnit();
            enemyUnits.Add(tempUnit);
            agents.Add(new Agent(tempUnit.GetComponent<Unit>(), texGenerator));
        }
        for (int num = 0; num < enemyMeleeUnitCount; num++)
        {
            GameObject tempUnit = SpawnMeleeEnemyUnit();
            enemyUnits.Add(tempUnit);
            agents.Add(new Agent(tempUnit.GetComponent<Unit>(), texGenerator));
        }

        for (int num = 0; num < qEnemyRangedUnitCount; num++)
        {
            GameObject tempUnit = SpawnQRangedEnemyUnit();
            qEnemyUnits.Add(tempUnit);
            qagents.Add(new QLearner(tempUnit.GetComponent<Unit>(), texGenerator));
        }
        for (int num = 0; num < qEnemyMeleeUnitCount; num++)
        {
            GameObject tempUnit = SpawnQMeleeEnemyUnit();
            qEnemyUnits.Add(tempUnit);
            qagents.Add(new QLearner(tempUnit.GetComponent<Unit>(), texGenerator));
        }
    }

    GameObject SpawnQMeleeEnemyUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("QCharcoalUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().type = Unit.Type.Water;
            unitPrefab.transform.position = new Vector3(x, y, z);

            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnQRangedEnemyUnit();
        }
        return unitPrefab;
    }

    GameObject SpawnQRangedEnemyUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("QPencilUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().type = Unit.Type.Oil;
            unitPrefab.transform.position = new Vector3(x, y, z);

            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnQRangedEnemyUnit();
        }
        return unitPrefab;
    }

    GameObject SpawnRangedPlayerUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("WaterBrushUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().type = Unit.Type.Water;
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

            unitPrefab = Instantiate(Resources.Load("OilBrushUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().type = Unit.Type.Oil;
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

            unitPrefab = Instantiate(Resources.Load("CharcoalUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().type = Unit.Type.Charcoal;
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

            unitPrefab = Instantiate(Resources.Load("PencilUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.GetComponent<Unit>().type = Unit.Type.Pencil;
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
        if (playerTurn == 1)
        {
            unitsList = playerUnits;
            remainingAP = totalPlayerAP;
        }
        else if(playerTurn == 2)
        {
            unitsList = enemyUnits;
            remainingAP = totalEnemyAP;
        }
        else
        {
            unitsList = qEnemyUnits;
            remainingAP = totalQEnemyAP;
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
        if (playerTurn == 1)
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
        if (playerTurn == 1)
        {
            unitsList = playerUnits;
        }
        else if(playerTurn == 2)
        {
            unitsList = enemyUnits;
        }
        else
        {
            unitsList = qEnemyUnits;
        }
        unitsList[curUnitInd].GetComponent<Unit>().ability = Unit.Ability.Attack;
        //playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = true;
        //playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = false;
        //playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = false;
    }

    public void EnableTar()
    {
        List<GameObject> unitsList;
        if (playerTurn == 1)
        {
            unitsList = playerUnits;
        }
        else if (playerTurn == 2)
        {
            unitsList = enemyUnits;
        }
        else
        {
            unitsList = qEnemyUnits;
        }
        unitsList[curUnitInd].GetComponent<Unit>().ability = Unit.Ability.Tar;
        //playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = false;
        //playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = true;
        //playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = false;
    }

    public void EnableMove()
    {
        List<GameObject> unitsList;
        if (playerTurn == 1)
        {
            unitsList = playerUnits;
        }
        else if (playerTurn == 2)
        {
            unitsList = enemyUnits;
        }
        else
        {
            unitsList = qEnemyUnits;
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

    void ResetUnitsAP(int player)
    {
        List<GameObject> unitsList;
        if (player == 1)
        {
            unitsList = playerUnits;
        }
        else if (playerTurn == 2)
        {
            unitsList = enemyUnits;
        }
        else
        {
            unitsList = qEnemyUnits;
        }
        for (int i = 0; i < unitsList.Count; i++)
        {
            unitsList[i].GetComponent<Unit>().ResetAP();
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0 * 30, 300, 300), "Player Points: " + playerPoints);
        GUI.Label(new Rect(0, 1 * 30, 300, 300), "Enemy Points: " + enemyPoints);
        GUI.Label(new Rect(0, 2 * 30, 300, 300), "Q Points: " + qEnemyPoints);
        if (gameEnd)
        {
            if(winner == 0)
            {
                GUI.Label(new Rect(0, 3 * 30, 300, 300), "Game Tied");
            }
            else if(winner == 1)
            {
                GUI.Label(new Rect(0, 3 * 30, 300, 300), "Player Won");
            }
            else if(winner == 2)
            {
                GUI.Label(new Rect(0, 3 * 30, 300, 300), "Enemy Won");
            }
            else if(winner == 3)
            {
                GUI.Label(new Rect(0, 3 * 30, 300, 300), "Q Enemy Won");
            }
        }
        GUI.Label(new Rect(0, 4 * 30, 300, 300), "Turns Remaining: " + numTurns);
    }

    void UpdateQValues()
    {
        for(int i = 0; i < qagents.Count; i++)
        {
            qagents[i].SaveQToFileOverwrite();
        }
    }

    void RecordLoss()
    {
        
        for (int i = 0; i < qagents.Count; i++)
        {
            qagents[i].IncrementLosingStreak();
            qagents[i].SaveQToFileOverwrite();
        }
    }
}
