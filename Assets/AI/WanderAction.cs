using UnityEngine;
using System.Collections;
/// <summary>
/// WanderAction makes the Unit move to a random tile within attack range.
/// </summary>
public class WanderAction : Action
{
    MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    float reward;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Execute(Unit u)
    {

        GameObject target = new GameObject();
        float x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, mapGen.MAP_WIDTH / 2);
        float y;
        float z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, mapGen.MAP_LENGTH / 2);
        x = Mathf.Clamp(x, u.gameObject.transform.position.x - u.attRange, u.gameObject.transform.position.x + u.attRange);
        z = Mathf.Clamp(z, u.gameObject.transform.position.z - u.attRange, u.gameObject.transform.position.z + u.attRange);
        y = mapGen.map[new Coordinate(x, z)].transform.position.y;
        target.transform.position = new Vector3(x, y, z);

        u.MoveToTarget(target);
    }

    public float GetReward()
    {
        return reward;
    }

    public string GetName()
    {
        return "Wander";
    }
}
