using UnityEngine;
using System.Collections;

public class RangedWeapon : MonoBehaviour {

    public GameObject target;
    public enum Type
    {
        Pencil,
        Charcoal,
        Water,
        Oil
    };

    public Type type;
    public TextureGenerator.Faction faction;
    GameObject projectilePrefab;
    float timeToTarget = 1.0f;
    float destroyTimer = 10.0f;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () { 

    }
    /// <summary>
    /// Fire corresponding projectile based on weapon type.
    /// </summary>
    /// <param name="t">Weapon type.</param>
    public void FireWeapon(Type t)
    {
        if (t == Type.Pencil)
        {
            projectilePrefab = Instantiate(Resources.Load("PencilProjectile", typeof(GameObject))) as GameObject;
            projectilePrefab.GetComponent<PencilProjectile>().faction = faction;
        }
        if (t == Type.Charcoal)
        {
            projectilePrefab = Instantiate(Resources.Load("CharcoalProjectile", typeof(GameObject))) as GameObject;
            projectilePrefab.GetComponent<CharcoalProjectile>().faction = faction;
        }
        if (t == Type.Water)
        {
            projectilePrefab = Instantiate(Resources.Load("WaterProjectile", typeof(GameObject))) as GameObject;
            projectilePrefab.GetComponent<WaterProjectile>().faction = faction;
        }
        if (t == Type.Oil)
        {
            projectilePrefab = Instantiate(Resources.Load("OilProjectile", typeof(GameObject))) as GameObject;
            projectilePrefab.GetComponent<OilProjectile>().faction = faction;
        }
        projectilePrefab.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        projectilePrefab.GetComponent<Rigidbody>().AddForce(CalculateThrowSpeed(this.transform.position, target.transform.position, timeToTarget), ForceMode.VelocityChange);
        Destroy(projectilePrefab, destroyTimer);
    }

    public void FireTar()
    {
        projectilePrefab = Instantiate(Resources.Load("TarProjectile", typeof(GameObject))) as GameObject;

        projectilePrefab.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        projectilePrefab.GetComponent<Rigidbody>().AddForce(CalculateThrowSpeed(this.transform.position, target.transform.position, timeToTarget), ForceMode.VelocityChange);
        Destroy(projectilePrefab, destroyTimer);
    }
    /// <summary>
    /// Calculates projectile speed required to reach target destination.
    /// </summary>
    /// <param name="orig">Original position.</param>
    /// <param name="targ">Target position.</param>
    /// <param name="timeToTarg">Amount of time that the projectile has to reach the target.</param>
    /// <returns></returns>
    Vector3 CalculateThrowSpeed(Vector3 orig, Vector3 targ, float timeToTarg)
    {
        Vector3 toTarget = targ - orig;
        Vector3 toTargetXZ = toTarget;
        //toTargetXZ.y = 0;

        float y = toTargetXZ.y;
        float xz = toTargetXZ.magnitude;

        float t = timeToTarg;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        Vector3 result = toTargetXZ.normalized;
        result *= v0xz;
        result.y = v0y;
        return result;        
    }

    /// <summary>
    /// Velocity vector for the projectile to be able to reach target. CURRENTLY NOT USED.
    /// </summary>
    /// <param name="targ"></param>
    /// <param name="angle"></param>
    /// <returns></returns>
    Vector3 BallisticVelocity(GameObject targ, float angle)
    {
        Vector3 direction = targ.transform.position - transform.position;
        float height = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float ang = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(ang);
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * ang));
        return velocity * direction.normalized;
    }
}
