using UnityEngine;
using System.Collections;

public class WanderSEAction : Action
{
    MapGenerator mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    float reward;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Execute(Unit u)
    {
        Debug.Log("Wandering SE");
        GameObject target = new GameObject();
        float x;
        float y;
        float z;

        x = Random.Range(0, mapGen.MAP_WIDTH / 2);
        z = Random.Range(mapGen.MAP_LENGTH / 2 * -1, 0);

        x = Mathf.Clamp(x, u.gameObject.transform.position.x - u.attRange, u.gameObject.transform.position.x + u.attRange);
        z = Mathf.Clamp(z, u.gameObject.transform.position.z - u.attRange, u.gameObject.transform.position.z + u.attRange);
        y = mapGen.map[new Coordinate(x, z)].transform.position.y;
        target.transform.position = new Vector3(x, y, z);
        //u.ability = Unit.Ability.Attack;
        u.MoveToTarget(target);
    }

    public float GetReward()
    {
        return reward;
    }
}
