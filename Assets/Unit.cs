using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour{
    [SerializeField]
    float attRange;
    [SerializeField]
    int damage;
    [SerializeField]
    int health;
    [SerializeField]
    GameObject weapon;
    GameObject prevTile;

    public enum Ability
    {
        None,
        Move,
        Attack,
        Tar
    };

    public Ability ability;
    int enemyInd;
    ArrayList availEnem;
    //public bool attackAbil;
    //public bool tarAbil;
    //public bool moveAbil;
    public bool active;
    public int AP;
    public float mobilityDist;

    void Awake()
    {
        ability = Ability.Move;
        availEnem = new ArrayList();
        //damage = 2;
        //health = 5;
        enemyInd = 0;
        active = false;
        //attackAbil = false;
        //tarAbil = false;
        //moveAbil = true;
        AP = 2;
        attRange = 8;
        mobilityDist = 6;
    }
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region old calls to when we used to attack other Units
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
        #endregion
        if (active)
        {
            HandleInput();
        }
    }

    /// <summary>
    /// Constructor for Unit with attack range initializer.
    /// </summary>
    /// <param name="r">Attack range in float.</param>
    Unit(float r)
    {
        attRange = r;
    }
    
    /// <summary>
    /// Handles click events from the player. 
    /// </summary>
    void HandleInput()
    {
        if (/*Input.GetMouseButtonDown(0) &&*/ !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("LMB Down");
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {
                Debug.Log("Hit " + hitInfo.transform.gameObject.name);
                Debug.Log(hitInfo.transform.gameObject.GetComponent<Renderer>().material.name);

                ExecuteAbility(hitInfo);

                if (hitInfo.transform.gameObject.tag == "GroundTile")
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

    /// <summary>
    /// Calls appropriate ability on hitInfo tile depending on active ability
    /// </summary>
    /// <param name="hitInfo">Ray that we derive target GameObject from</param>
    void ExecuteAbility(RaycastHit hitInfo)
    {
        if (prevTile == null)
        {
            prevTile = hitInfo.transform.gameObject;
        }
        if (prevTile != hitInfo.transform.gameObject)
        {
            prevTile.GetComponent<Renderer>().material.color = Color.white;
        }
        switch(ability)
        {
            case Ability.Attack:
                if (Input.GetMouseButtonDown(0) && CalculateDistance(gameObject.transform.position, hitInfo.transform.gameObject.transform.position) < attRange)
                {
                    AttackTarget(hitInfo.transform.gameObject);
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
                }
                else if(CalculateDistance(gameObject.transform.position, hitInfo.transform.gameObject.transform.position) < attRange)
                {
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                }
                break;
            case Ability.Tar:
                if (Input.GetMouseButtonDown(0) && CalculateDistance(gameObject.transform.position, hitInfo.transform.gameObject.transform.position) < attRange)
                {
                    AttackTarget(hitInfo.transform.gameObject);
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.black;
                }
                else if (CalculateDistance(gameObject.transform.position, hitInfo.transform.gameObject.transform.position) < attRange)
                {
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.gray;
                }
                break;
            case Ability.Move:
                if (Input.GetMouseButtonDown(0) && CalculateDistance(gameObject.transform.position, hitInfo.transform.gameObject.transform.position) < mobilityDist)
                {
                    MoveToTarget(hitInfo.transform.gameObject);
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.green;
                }
                else if(CalculateDistance(gameObject.transform.position, hitInfo.transform.gameObject.transform.position) < mobilityDist)
                {
                    hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                }
                break;
            default:
                break;
        }
        #region old ability switching using booleans
        /*if (attackAbil)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttackTarget(hitInfo.transform.gameObject);
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            }
        }
        if (tarAbil)
        {
            if (Input.GetMouseButtonDown(0))
            {
                AttackTarget(hitInfo.transform.gameObject);
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.black;
            }
            else
            {
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.gray;
            }
        }
        if (moveAbil)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MoveToTarget(hitInfo.transform.gameObject);
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                hitInfo.transform.gameObject.GetComponent<Renderer>().material.color = Color.blue;
            }
        }*/
        #endregion
        prevTile = hitInfo.transform.gameObject;
    }

    /// <summary>
    /// Calls SetTarget to set weapon target and then fire weapon
    /// </summary>
    /// <param name="targ">target GameObject</param>
    void AttackTarget(GameObject targ)
    {
        SetTarget(targ);
        weapon.GetComponent<RangedWeapon>().FireWeapon();
    }

    /// <summary>
    /// Move to target GameObject's position, decrementing AP according to distance travelled
    /// </summary>
    /// <param name="targ">Target GameObject</param>
    void MoveToTarget(GameObject targ)
    {
        if (CalculateDistance(gameObject.transform.position, targ.transform.position) <= mobilityDist / 2.0f)
        {
            gameObject.transform.position = targ.transform.position;
            AP -= 1;
        }
        else if (CalculateDistance(gameObject.transform.position, targ.transform.position) <= mobilityDist)
        {
            gameObject.transform.position = targ.transform.position;
            AP -= 2;
        }

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

    /// <summary>
    /// Sets target of the weapon of this Unit.
    /// </summary>
    /// <param name="targ"> GameObject target that we'll set the weapon target to.</param>
    void SetTarget(GameObject targ)
    {
        weapon.GetComponent<RangedWeapon>().target = targ;
    }

    #region old code that allows for enemy detection from when we attack enemy units
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    //
    // PAST THIS POINT IS THE OLD CODE FOR WHEN WE USED TO ATTACK OTHER UNITS.
    // CODE IS KEPT IN CASE WE NEED TO USE SOMETHING SIMILAR.
    //
    //////////////////////////////////////////////////////////////////////////////////////////////////////
    /**
    * Scans all objects within range zone to add all attackable enemies into available enemies list.
    **/
    bool TargetSearch()
    {
        bool hit = false;
        Collider[] hitColliders = Physics.OverlapSphere(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1, gameObject.transform.position.z), attRange);
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
    #endregion
}
