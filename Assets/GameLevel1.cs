using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLevel1 : MonoBehaviour {
    [SerializeField]
    GameObject mapGenerator;
    List<GameObject> playerUnits = new List<GameObject>();
    static int playerUnitCount = 2;
    public int curUnitInd;
    MapGenerator mapGen;

    void Awake()
    {

    }
    // Use this for initialization
    void Start ()
    {
        mapGen = mapGenerator.GetComponent<MapGenerator>();
        mapGen.GenerateMap();
        SpawnUnits();
        curUnitInd = 0;
        EnableUnit(playerUnits, curUnitInd);
    }
	
	// Update is called once per frame
	void Update () {
        if(playerUnits[curUnitInd].GetComponent<Unit>().AP <= 0)
        {
            if(!SelectNextUnit())
            {
                Debug.Log("Turn Ended");
            }
        }
	}

    void SpawnUnits()
    {
        for (int num = 0; num < playerUnitCount; num++)
        {
            playerUnits.Add(SpawnPlayerUnit());
        }
    }

    GameObject SpawnPlayerUnit()
    {
        GameObject unitPrefab;
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        if (!mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied)
        {
            float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

            unitPrefab = Instantiate(Resources.Load("PencilUnitPrefab", typeof(GameObject))) as GameObject;
            unitPrefab.transform.position = new Vector3(x, y, z);
            mapGen.map[new Coordinate(x, z)].GetComponent<Tile>().occupied = true;
        }
        else
        {
            return SpawnPlayerUnit();
        }
        return unitPrefab;
    }

    public bool SelectNextUnit()
    {
        if (curUnitInd < playerUnits.Count -1)
        {
            DisableUnit(playerUnits, curUnitInd);
            curUnitInd++;
            EnableUnit(playerUnits, curUnitInd);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SelectPreviousUnit()
    {
        if (curUnitInd > 0)
        {
            DisableUnit(playerUnits, curUnitInd);
            curUnitInd--;
            EnableUnit(playerUnits, curUnitInd);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void EnableAttack()
    {
        playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = true;
        playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = false;
        playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = false;
    }

    public void EnableTar()
    {
        playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = false;
        playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = true;
        playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = false;
    }

    public void EnableMove()
    {
        playerUnits[curUnitInd].GetComponent<Unit>().attackAbil = false;
        playerUnits[curUnitInd].GetComponent<Unit>().tarAbil = false;
        playerUnits[curUnitInd].GetComponent<Unit>().moveAbil = true;
    }

    void EnableUnit(List<GameObject> units, int index)
    {
        units[index].GetComponent<Unit>().active = true;
    }

    void DisableUnit(List<GameObject> units, int index)
    {
        units[index].GetComponent<Unit>().active = false;
    }
}
