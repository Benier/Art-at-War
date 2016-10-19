using UnityEngine;
using System.Collections;

public class WanderNWAction : Action
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
        GameObject target = new GameObject();
        float x;
        float y;
        float z;

        x = Random.Range(mapGen.MAP_WIDTH / 2 * -1, 0);
        z = Random.Range(0, mapGen.MAP_LENGTH / 2);

        x = Mathf.Clamp(x, u.gameObject.transform.position.x - u.attRange, u.gameObject.transform.position.x + u.attRange);
        z = Mathf.Clamp(z, u.gameObject.transform.position.z - u.attRange, u.gameObject.transform.position.z + u.attRange);
        y = mapGen.map[new Coordinate(x, z)].transform.position.y;
        target.transform.position = new Vector3(x, y, z);

        reward = CalculateDistance(target.transform.position, new Vector3());
        u.MoveToTarget(target);
    }

    public float GetReward()
    {
        return reward;
    }

    public string GetName()
    {
        return "WanderNWAction";
    }

    /// <summary>
    /// Calculates distance between two points
    /// </summary>
    /// <param name="orig">Origin point</param>
    /// <param name="dest">Destination point</param>
    /// <returns>float distance between two points using pythagoras</returns>
    float CalculateDistance(Vector3 orig, Vector3 dest)
    {
        return Mathf.Sqrt(Mathf.Pow((dest.x - orig.x), 2.0f) + Mathf.Pow((dest.z - orig.z), 2.0f));
    }
}
