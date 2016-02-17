using UnityEngine;
using System.Collections;

public class RangedWeapon : MonoBehaviour {

    [SerializeField]
    GameObject target;
    [SerializeField]
    GameObject projectile;

    GameObject projectilePrefab;
    float timeToTarget = 1.0f;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () { 

        if (Input.GetMouseButtonDown(0))
        {
            projectilePrefab = Instantiate(Resources.Load("Projectile", typeof(GameObject))) as GameObject;
            //projectilePrefab.GetComponent<Rigidbody>().velocity = BallisticVelocity(target, 60.0f);
            //Vector3 temp = projectilePrefab.GetComponent<Rigidbody>().velocity;
            projectilePrefab.transform.position = this.transform.position;
            projectilePrefab.GetComponent<Rigidbody>().AddForce(calculateThrowSpeed(this.transform.position, target.transform.position, timeToTarget), ForceMode.VelocityChange);
        }
    }

    Vector3 calculateThrowSpeed(Vector3 orig, Vector3 targ, float timeToTarg)
    {
        Vector3 toTarget = targ - orig;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

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
