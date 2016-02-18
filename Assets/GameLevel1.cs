using UnityEngine;
using System.Collections;

public class GameLevel1 : MonoBehaviour {
    [SerializeField]
    GameObject mapGenerator;

    MapGenerator mapGen;
	// Use this for initialization
	void Start () {
        mapGen = mapGenerator.GetComponent<MapGenerator>();
        mapGen.GenerateMap();
        SpawnPlayerUnit();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SpawnPlayerUnit()
    {
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        float y = mapGen.map[new Coordinate(x, z)].transform.position.y;        //get height of map tile on that tile in map

        GameObject unitPrefab = Instantiate(Resources.Load("PencilUnitPrefab", typeof(GameObject))) as GameObject;
        unitPrefab.transform.position = new Vector3(x, y, z);
    }
}
