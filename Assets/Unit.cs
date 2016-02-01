using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour{
    float range = 2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
        if(Input.GetKey(KeyCode.R))
        {
            Attack();
        }
	}

    Unit(float r)
    {
        range = r;
    }

    bool Attack()
    {
        bool hit = false;
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), range);
        int i = 0;
        while(i < hitColliders.Length)
        {
            GameObject enemy = hitColliders[i].gameObject;
            if (enemy.GetComponent<Agent>() != null)
            {
                Debug.Log("Ayyyy");
            }
            i++;
        }
        return hit;
    }
}
