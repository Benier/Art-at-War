using UnityEngine;
using System.Collections;
using System.Timers;

/// <summary>
/// OLD HAND CODED PROJECTILE PHYSICS CLASS FROM THE PHYSICS COURSE. NOT USED.
/// </summary>
public class ProjectilePhysics : MonoBehaviour
{
    float colliPrec;
    bool upArr;
    bool downArr;
    bool rightArr;
    bool leftArr;
    bool startTime;
    bool resetPos;
    bool shotFired;
    bool velocityUp;
    bool velocityDown;
    bool gunRotZ;
    bool gunRotY;
    int hit;
    GUIStyle fontstyle;

    float ballPosX;
    float ballPosY;
    float ballPosZ;
    float ballPosXInit;
    float ballPosYInit;
    float ballPosZInit;

    public float targetPosX;
    public float targetPosY;
    public float targetPosZ;

    float gunPosX;
    float gunPosY;
    float gunPosZ;

    float velocityIX;
    float velocityFX;
    float velocityIY;
    float velocityFY;

    float velocity;
    float velocityX;
    float velocityY;
    float velocityZ;
    float velocityInit;

    float accelX;
    float accelY;
    float accelZ;

    Vector3 trajVec;
    Vector3 postRotVec;

    float deltaTime;
    float timerInit;
    float timer;
    float elapsedTime;
    Vector3 gunRot; //alpha, y, gamma
    float correctRotDegY;
    float correctRotDegZ;

    float rangeBoundX;
    public GameObject go;
    [SerializeField]
    ArrayList arc;
    [SerializeField]
    GameObject origin;
    [SerializeField]
    GameObject target;
    int frameCount;

    // Use this for initialization
    void Start()
    {
        colliPrec = 1.0f;
        fontstyle = new GUIStyle();
        fontstyle.fontSize = 60;
        fontstyle.normal.textColor = Color.white;
        arc = new ArrayList();
        timerInit = 8;
        timer = timerInit;
        rangeBoundX = 800;

        upArr = false;
        downArr = false;
        rightArr = false;
        leftArr = false;
        startTime = false;
        shotFired = false;
        velocityUp = false;
        velocityDown = false;
        hit = 0;

        trajVec = new Vector3(1, 0, 0);
        ballPosXInit = origin.transform.position.x;
        ballPosYInit = origin.transform.position.y + 2;
        ballPosZInit = origin.transform.position.z;
        ballPosX = ballPosXInit;
        ballPosY = ballPosYInit;
        ballPosZ = ballPosZInit;

        gunPosX = ballPosXInit;
        gunPosY = ballPosYInit;
        gunPosZ = ballPosZInit;

        targetPosX = target.transform.position.x;
        targetPosY = target.transform.position.y;
        targetPosZ = target.transform.position.z;

        gunRot = new Vector3(0, Mathf.Deg2Rad * 180.0f, 0);

        accelX = 0.0f;
        accelY = -9.81f;
        accelZ = 0.0f;
        velocityInit = 100.0f;
        velocity = velocityInit;
        velocityIX = (float)(velocity * Mathf.Cos(gunRot.z));
        velocityFX = velocityIX;
        velocityIY = (float)(velocity * Mathf.Sin(gunRot.z));
        velocityFY = Mathf.Abs(velocityIY);
        frameCount = 0;

        gameObject.transform.position = new Vector3(ballPosX, ballPosY, -ballPosZ);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(ballPosX, ballPosY, -ballPosZ);

        // TODO: Add your update logic here
        if (startTime)
        {
            if (!shotFired)
            {
                postRotVec = Quaternion.Euler(Mathf.Rad2Deg * gunRot.x, Mathf.Rad2Deg * gunRot.y, Mathf.Rad2Deg * gunRot.z) * trajVec;
                velocityX = postRotVec.x * velocity;
                velocityY = postRotVec.y * velocity;
                velocityZ = postRotVec.z * velocity;
                shotFired = true;
            }
            deltaTime = Time.fixedDeltaTime;
            timer -= deltaTime;
            elapsedTime += deltaTime;

            ballPosX += (float)(velocityX * deltaTime) + (float)(1.0f / 2.0f * accelX * Mathf.Pow(deltaTime, 2));
            velocityX += (float)(accelX * deltaTime);
            ballPosY += (float)(velocityY * deltaTime) + (float)(1.0f / 2.0f * accelY * Mathf.Pow(deltaTime, 2));
            velocityY += (float)(accelY * deltaTime);
            ballPosZ += (float)(velocityZ * deltaTime) + (float)(1.0f / 2.0f * accelZ * Mathf.Pow(deltaTime, 2));
            velocityZ += (float)(accelZ * deltaTime);

            if (ballPosY <= targetPosY)
            {
                startTime = false;
                shotFired = false;

                if ((Mathf.Abs(ballPosX - targetPosX) < colliPrec) && (Mathf.Abs(ballPosZ - targetPosZ) < colliPrec))
                {
                    hit = 1;
                }
                else
                {
                    hit = 2;
                }
            }
        }
        if (resetPos)
        {
            timer = timerInit;
            elapsedTime = 0;
            ballPosX = ballPosXInit;
            ballPosY = ballPosYInit;
            ballPosZ = ballPosZInit;
            velocity = velocityInit;
            resetPos = false;
            startTime = false;
            shotFired = false;
            hit = 0;

            foreach (GameObject g in arc)
            {
                Destroy(g);
            }
            arc.Clear();
        }
        correctRotDegY = (float)(Mathf.Rad2Deg * ((float)(Mathf.Acos(Mathf.Abs(targetPosX - gunPosX) / Mathf.Sqrt(Mathf.Pow(targetPosX - gunPosX, 2) + Mathf.Pow(targetPosZ - gunPosZ, 2))))));
        correctRotDegZ = (float)(Mathf.Rad2Deg * ((float)(Mathf.Asin(Mathf.Abs(accelY) * Mathf.Sqrt(Mathf.Pow((targetPosX - gunPosX), 2) + Mathf.Pow((targetPosZ - gunPosZ), 2)) / Mathf.Pow(velocity, 2)))) / 2);
        gunRot.y = Mathf.Deg2Rad * (360.0f - correctRotDegY);
        gunRot.z = Mathf.Deg2Rad * correctRotDegZ;

        if (frameCount > 5)
        {
            GameObject c = Instantiate(go, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            arc.Add(c);
            frameCount = 0;
        }
        frameCount++;

        CheckInput();
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            upArr = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) && upArr)
        {
            upArr = false;
            //velocity += 10.0f;
            targetPosZ -= 10.0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            downArr = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow) && downArr)
        {
            downArr = false;
            targetPosZ += 10.0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftArr = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) && leftArr)
        {
            leftArr = false;
            targetPosX -= 10.0f;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightArr = true;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow) && rightArr)
        {
            rightArr = false;
            targetPosX += 10.0f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startTime = true;
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            resetPos = true;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            velocityDown = true;
        }
        if (Input.GetKeyUp(KeyCode.Z) && velocityDown)
        {
            velocityDown = false;
            velocity -= 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            velocityUp = true;
        }
        if (Input.GetKeyUp(KeyCode.X) && velocityUp)
        {
            velocityUp = false;
            velocity += 1.0f;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0 * 30, 300, 300), "Projectile Position: (" + ballPosX + ", " + ballPosY + ", " + ballPosZ + ")");
        GUI.Label(new Rect(0, 1 * 30, 300, 300), "Target Position: (" + targetPosX + ", " + targetPosY + ", " + targetPosZ + ")");
        GUI.Label(new Rect(0, 2 * 30, 300, 300), "Range: (" + Mathf.Abs(ballPosX - targetPosX) + ", " + Mathf.Abs(ballPosY - targetPosY) + ", " + Mathf.Abs(ballPosZ - targetPosZ) + ")");
        GUI.Label(new Rect(0, 3 * 30, 300, 300), "Correct Alpha: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * (90 - correctRotDegZ)))));
        GUI.Label(new Rect(0, 4 * 30, 300, 300), "Correct Gamma: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * (correctRotDegY)))));
        GUI.Label(new Rect(0, 5 * 30, 300, 300), "Current Alpha: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * 90) - gunRot.z)));
        GUI.Label(new Rect(0, 6 * 30, 300, 300), "Current Gamma: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * 360) - gunRot.y)));
        GUI.Label(new Rect(0, 7 * 30, 300, 300), "Time: " + elapsedTime + " s");
        GUI.Label(new Rect(0, 8 * 30, 300, 300), "Velocity: " + velocity + " m/s");


        GUI.Label(new Rect(0, 10 * 30, 300, 300), "Hit or Miss with Drag and Wind: NA");

        if (hit == 1)
        {
            GUI.Label(new Rect(500, 1 * 30, 300, 300), "You hit it. So obvious, isn't it?", fontstyle);
        }
        else if (hit == 2)
        {
            GUI.Label(new Rect(500, 1 * 30, 300, 300), "Noo! All hell break loose!", fontstyle);
        }
    }
}
