using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour{
    [SerializeField]
    float range;
    [SerializeField]
    int damage;
    [SerializeField]
    int health;
    [SerializeField]
    GameObject weapon;
    int enemyInd;
    ArrayList availEnem;
	// Use this for initialization
	void Start () {
        availEnem = new ArrayList();
        //range = 2;
        //damage = 2;
        enemyInd = 0;
        //health = 5;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    // Old code for testing whether enemies are in range
        /*if(Input.GetKey(KeyCode.Alpha1))
        {
            TargetSearch();
        }
        if (availEnem.Count > 0)
        {
            // Increase enemy index
            if (Input.GetKey(KeyCode.RightArrow))
            {
                enemyInd++;
            }
            // Decrease enemy index
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                enemyInd--;
            }
            // Bound checking and correction for enemy index value
            if(enemyInd >= availEnem.Count)
            {
                enemyInd = availEnem.Count - 1;
            }
            if(enemyInd < 0)
            {
                enemyInd = 0;
            }
            // Attack enemy
            if (Input.GetKey(KeyCode.R))
            {
                Attack();
            }
        }*/
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("LMB Down");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if(hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                Debug.Log(hitInfo.transform.gameObject.GetComponent<Renderer>().material.name);
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
                SetTarget(hitInfo.transform.gameObject);
                weapon.GetComponent<RangedWeapon>().FireWeapon();
                if(hitInfo.transform.gameObject.tag == "GroundTile")
                {
                    Debug.Log("Hit Ground");
                }
                else
                {
                    Debug.Log("Hit something else");
                }
            }
            else
            {
                Debug.Log("Hit nothing");
            }
        }
    }

    Unit(float r)
    {
        range = r;
    }

    void SetTarget(GameObject targ)
    {
        weapon.GetComponent<RangedWeapon>().target = targ;
    }

    /**
    * Scans all objects within range zone to add all attackable enemies into available enemies list.
    **/
    bool TargetSearch()
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
                if (!availEnem.Contains(enemy.GetComponent<Agent>()))
                {
                    availEnem.Add(enemy.GetComponent<Agent>());
                }
            }
            i++;
        }
        return hit;
    }

    /**
    * Decrease enemy Agent health by damage. Returns true if enemy is dead, false if it's still alive.
    **/
    bool Attack()
    {
        ((Agent)availEnem[enemyInd]).health -= damage;
        if (((Agent)availEnem[enemyInd]).health <= 0)
        {
            availEnem.Clear();
            return true;
        }
        else
        {
            availEnem.Clear();
            return false;
        }
    }
}
