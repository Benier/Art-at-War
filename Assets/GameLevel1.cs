using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLevel1 : MonoBehaviour
{
    [SerializeField]
    public GameObject mapGenerator;
    [SerializeField]
    GameObject unitCounter;
    public GameObject loadingActTexture;
    public GameObject loadingEnemyActTexture;
    TextureGenerator texGenerator;
    GameObject optionsHolder;
    public Canvas endGameCanvas;
    public static Dictionary<Coordinate, GameObject> map;
    public GameObject curUnitLight;
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
    int totalEnemyCount;
    int numExecutedEnemies;
    public int curUnitInd;
    public int numQValStores;
    int playerTurn; // 1 = player, 2 = enemy, 3 = q enemy
    int totalPlayerAP;
    int totalEnemyAP;
    int totalQEnemyAP;
    int numTurns = 1;
    public int playerPoints;
    public int enemyPoints;
    public int qEnemyPoints;
    public float lightHoverHeight;
    public Text winLossText;
    int winner; //0 = none, 1 = player, 2 = enemy, 3 = q enemy
    bool gameEnd;

    MapGenerator mapGen;

    void Awake()
    {
        optionsHolder = GameObject.Find("OptionsHolder");
        numQValStores = optionsHolder.GetComponent<OptionsHolder>().maxIterations;
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
        unitCounter = GameObject.Find("UnitCounter");
        texGenerator = GameObject.Find("TexGenerator").GetComponent<TextureGenerator>();
        endGameCanvas = GameObject.Find("EndGameCanvas").GetComponent<Canvas>();
        endGameCanvas.enabled = false;
        map = mapGen.GenerateMap(texGenerator);

        playerRangedUnitCount = unitCounter.GetComponent<UnitCounter>().numPlayer;
        playerMeleeUnitCount = 0;
        enemyRangedUnitCount = unitCounter.GetComponent<UnitCounter>().numEnemy;
        enemyMeleeUnitCount = 0;
        qEnemyRangedUnitCount = unitCounter.GetComponent<UnitCounter>().numQEnemy;
        qEnemyMeleeUnitCount = 0;
        numTurns = unitCounter.GetComponent<UnitCounter>().numTurns;

        SpawnUnits();
        curUnitInd = 0;
        EnableUnit(playerUnits, curUnitInd);
        playerTurn = 1;
        gameEnd = false;
        lightHoverHeight = 2.0f;
        totalEnemyCount = enemyRangedUnitCount + enemyMeleeUnitCount + qEnemyRangedUnitCount + qEnemyMeleeUnitCount;

        ColourRangeTiles(playerUnits[curUnitInd], playerUnits[curUnitInd].GetComponent<Unit>().attRange);
    }

    // Update is called once per frame
    void Update ()
    {
        if(!texGenerator.generating)
        {
            StartCoroutine("UpdateCoroutine");
        }
        
    }

    void LateUpdate()
    {
        if(texGenerator.generating) //if it's not player's turn
        {
            if (playerTurn != 1)
            {
                loadingEnemyActTexture.SetActive(true);
            }
            else
            {
                loadingActTexture.SetActive(true);
            }
        }
        else
        {
            loadingActTexture.SetActive(false);
            loadingEnemyActTexture.SetActive(false);
        }
        if(gameEnd)
        {
            endGameCanvas.enabled = true;
        }
    }

    IEnumerator UpdateCoroutine()
    {
        if (numTurns > 0)
        {
            if (numExecutedEnemies == totalEnemyCount)
            {
                numExecutedEnemies = 0;
            }
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
                curUnitLight.transform.position = new Vector3(playerUnits[curUnitInd].transform.position.x, playerUnits[curUnitInd].transform.position.y + lightHoverHeight, playerUnits[curUnitInd].transform.position.z);
                if (playerUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
                {
                    if (!SelectNextUnit() && !texGenerator.generating)
                    {
                        ClearTiles();
                        //Debug.Log("Turn Ended");
                        curUnitInd = 0;
                        if (enemyUnits.Count > 0)
                        {
                            EnableUnit(enemyUnits, curUnitInd);
                            playerTurn = 2;
                            EnableMove();
                            EnableAttack();
                            EnableTar();
                        }
                        else
                        {
                            EnableUnit(qEnemyUnits, curUnitInd);
                            playerTurn = 3;
                            EnableMove();
                            EnableAttack();
                            EnableTar();
                        }
                        ResetUnitsAP(playerTurn);
                    }
                }
            }
            else if (playerTurn == 2)
            {
                curUnitLight.transform.position = new Vector3(enemyUnits[curUnitInd].transform.position.x, 
                    enemyUnits[curUnitInd].transform.position.y + lightHoverHeight, 
                    enemyUnits[curUnitInd].transform.position.z);
                agents[curUnitInd].Update();

                if (enemyUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
                {
                    if (!SelectNextUnit() && !texGenerator.generating)
                    {
                        //Debug.Log("Turn Ended");
                        curUnitInd = 0;

                        if (qEnemyUnits.Count > 0)
                        {
                            EnableUnit(qEnemyUnits, curUnitInd);
                            playerTurn = 3;
                            EnableMove();
                            EnableAttack();
                            EnableTar();
                        }
                        else
                        {
                            EnableUnit(playerUnits, curUnitInd);
                            playerTurn = 1;
                            EnableMove();
                            EnableAttack();
                            EnableTar();
                            numTurns--;
                            ColourRangeTiles(playerUnits[curUnitInd], playerUnits[curUnitInd].GetComponent<Unit>().attRange);
                        }
                        ResetUnitsAP(playerTurn);
                        //numTurns--;
                    }
                    numExecutedEnemies++;
                }
            }
            else if (playerTurn == 3)
            {
                curUnitLight.transform.position = new Vector3(qEnemyUnits[curUnitInd].transform.position.x, 
                    qEnemyUnits[curUnitInd].transform.position.y + lightHoverHeight, 
                    qEnemyUnits[curUnitInd].transform.position.z);
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
                        EnableAttack();
                        EnableTar();
                        ResetUnitsAP(playerTurn);
                        numTurns--;
                        ColourRangeTiles(playerUnits[curUnitInd], playerUnits[curUnitInd].GetComponent<Unit>().attRange);
                    }
                    numExecutedEnemies++;
                }
            }
            totalPlayerAP = 0;
            totalEnemyAP = 0;
        }
        else
        {
            if (numExecutedEnemies == totalEnemyCount && !texGenerator.generating)
            {
                if (playerPoints > enemyPoints)
                {
                    winner = 1;
                }
                else if (playerPoints < enemyPoints)
                {
                    winner = 2;
                }
                else
                {
                    winner = 0;
                }

                gameEnd = true;
            }
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
        yield return null;
    }

    public void CheckWinner()
    {
        if (numExecutedEnemies == totalEnemyCount && !texGenerator.generating)
        {
            if (playerPoints > enemyPoints)
            {
                winner = 1;
            }
            else if (playerPoints < enemyPoints)
            {
                winner = 2;
            }
            else
            {
                winner = 0;
            }

            if (numTurns <= 0)
            {
                gameEnd = true;
            }
        }
    }

    void ColourRangeTiles(GameObject unit, float range)
    {
        foreach (KeyValuePair<Coordinate, GameObject> b in map)
        {
            if(Mathf.Sqrt(Mathf.Pow((b.Value.transform.position.x - unit.transform.position.x), 2.0f) 
                + Mathf.Pow((b.Value.transform.position.z - unit.transform.position.z), 2.0f)) < range)
            {
                b.Value.GetComponent<Renderer>().material.color = Color.cyan;
            }
            else
            {
                b.Value.GetComponent<Renderer>().material.color = Color.white;
            }
        }
    }

    void ClearTiles()
    {
        foreach (KeyValuePair<Coordinate, GameObject> b in map)
        {
            b.Value.GetComponent<Renderer>().material.color = Color.white;
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
                RecordWin();
                UpdateQValues();
            }            
        }
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        EndGameSaveQ();
        DontDestroyOnLoad(unitCounter);
        DontDestroyOnLoad(optionsHolder);
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgain()
    {
        EndGameSaveQ();
        DontDestroyOnLoad(unitCounter);
        DontDestroyOnLoad(optionsHolder);
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
                RecordWin();
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

            unitPrefab = Instantiate(Resources.Load(unitCounter.GetComponent<UnitCounter>().qEnemyFaction, typeof(GameObject))) as GameObject;
            Unit.Type projectileType = Unit.Type.Pencil;

            if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "PencilUnitPrefab")
            {
                projectileType = Unit.Type.Pencil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "CharcoalUnitPrefab")
            {
                projectileType = Unit.Type.Charcoal;
            }
            else if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "OilBrushUnitPrefab")
            {
                projectileType = Unit.Type.Oil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "WaterBrushUnitPrefab")
            {
                projectileType = Unit.Type.Water;
            }
            unitPrefab.GetComponent<Unit>().type = projectileType;
            unitPrefab.GetComponent<Unit>().faction = TextureGenerator.Faction.Enemy;
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

            unitPrefab = Instantiate(Resources.Load(unitCounter.GetComponent<UnitCounter>().qEnemyFaction, typeof(GameObject))) as GameObject;
            Unit.Type projectileType = Unit.Type.Pencil;

            if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "PencilUnitPrefab")
            {
                projectileType = Unit.Type.Pencil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "CharcoalUnitPrefab")
            {
                projectileType = Unit.Type.Charcoal;
            }
            else if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "OilBrushUnitPrefab")
            {
                projectileType = Unit.Type.Oil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().qEnemyFaction == "WaterBrushUnitPrefab")
            {
                projectileType = Unit.Type.Water;
            }
            unitPrefab.GetComponent<Unit>().type = projectileType;
            unitPrefab.GetComponent<Unit>().faction = TextureGenerator.Faction.Enemy;
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

            unitPrefab = Instantiate(Resources.Load(unitCounter.GetComponent<UnitCounter>().playerFaction, typeof(GameObject))) as GameObject;
            Unit.Type projectileType = Unit.Type.Pencil;

            if (unitCounter.GetComponent<UnitCounter>().playerFaction == "PencilUnitPrefab")
            {
                projectileType = Unit.Type.Pencil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().playerFaction == "CharcoalUnitPrefab")
            {
                projectileType = Unit.Type.Charcoal;
            }
            else if (unitCounter.GetComponent<UnitCounter>().playerFaction == "OilBrushUnitPrefab")
            {
                projectileType = Unit.Type.Oil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().playerFaction == "WaterBrushUnitPrefab")
            {
                projectileType = Unit.Type.Water;
            }
            unitPrefab.GetComponent<Unit>().type = projectileType;
            unitPrefab.GetComponent<Unit>().faction = TextureGenerator.Faction.Player;
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

            unitPrefab = Instantiate(Resources.Load(unitCounter.GetComponent<UnitCounter>().playerFaction, typeof(GameObject))) as GameObject;
            Unit.Type projectileType = Unit.Type.Pencil;

            if (unitCounter.GetComponent<UnitCounter>().playerFaction == "PencilUnitPrefab")
            {
                projectileType = Unit.Type.Pencil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().playerFaction == "CharcoalUnitPrefab")
            {
                projectileType = Unit.Type.Charcoal;
            }
            else if (unitCounter.GetComponent<UnitCounter>().playerFaction == "OilBrushUnitPrefab")
            {
                projectileType = Unit.Type.Oil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().playerFaction == "WaterBrushUnitPrefab")
            {
                projectileType = Unit.Type.Water;
            }
            unitPrefab.GetComponent<Unit>().type = projectileType;
            unitPrefab.GetComponent<Unit>().faction = TextureGenerator.Faction.Player;
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

            unitPrefab = Instantiate(Resources.Load(unitCounter.GetComponent<UnitCounter>().enemyFaction, typeof(GameObject))) as GameObject;
            Unit.Type projectileType = Unit.Type.Pencil;

            if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "PencilUnitPrefab")
            {
                projectileType = Unit.Type.Pencil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "CharcoalUnitPrefab")
            {
                projectileType = Unit.Type.Charcoal;
            }
            else if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "OilBrushUnitPrefab")
            {
                projectileType = Unit.Type.Oil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "WaterBrushUnitPrefab")
            {
                projectileType = Unit.Type.Water;
            }
            unitPrefab.GetComponent<Unit>().type = projectileType;
            unitPrefab.GetComponent<Unit>().faction = TextureGenerator.Faction.Enemy;
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

            unitPrefab = Instantiate(Resources.Load(unitCounter.GetComponent<UnitCounter>().enemyFaction, typeof(GameObject))) as GameObject;
            Unit.Type projectileType = Unit.Type.Pencil;

            if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "PencilUnitPrefab")
            {
                projectileType = Unit.Type.Pencil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "CharcoalUnitPrefab")
            {
                projectileType = Unit.Type.Charcoal;
            }
            else if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "OilBrushUnitPrefab")
            {
                projectileType = Unit.Type.Oil;
            }
            else if (unitCounter.GetComponent<UnitCounter>().enemyFaction == "WaterBrushUnitPrefab")
            {
                projectileType = Unit.Type.Water;
            }
            unitPrefab.GetComponent<Unit>().type = projectileType;
            unitPrefab.GetComponent<Unit>().faction = TextureGenerator.Faction.Enemy;
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
            if(playerTurn == 1)
            {
                ColourRangeTiles(unitsList[curUnitInd], unitsList[curUnitInd].GetComponent<Unit>().attRange);
            }
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
        if (gameEnd)
        {
            if(winner == 0)
            {
                winLossText.text = "Tie Game";
            }
            else if(winner == 1)
            {
                winLossText.text = "You Win";
            }
            else if(winner == 2)
            {
                winLossText.text = "You Lost";
            }
            else if(winner == 3)
            {
                winLossText.text = "You Lost";
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

    void RecordWin()
    {
        for (int i = 0; i < qagents.Count; i++)
        {
            qagents[i].ResetLosingStreak();
        }
    }
}
