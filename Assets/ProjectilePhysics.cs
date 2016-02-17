using UnityEngine;
using System.Collections;
using System.Timers;

public class ProjectilePhysics : MonoBehaviour
{
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
    float gunRotDegY;
    float gunRotDegZ;
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

    //------------Asn8
    int ballMass;
    float drag; //Cd
    float windVel;
    float windDir;
    float windC; //Cw
    bool dragOn;
    float tau;
    float gravity;

    // Use this for initialization
    void Start()
    {
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
        ballPosYInit = origin.transform.position.y;
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

        gunRotDegZ = 0;
        gunRot = new Vector3(0, Mathf.Deg2Rad * gunRotDegY, Mathf.Deg2Rad * gunRotDegZ);

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

        //--------------------Asn8
        ballMass = 1;
        drag = 0.2f;
        windVel = 10;
        windDir = ((Mathf.Deg2Rad * 360) - gunRot.y);
        windC = 0.1f;
        dragOn = false;
        tau = ballMass / drag;
        gravity = 9.81f;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(ballPosX, ballPosY, -ballPosZ);

        // TODO: Add your update logic here
        if (startTime)
        {
            if (!shotFired)
            {
                postRotVec = Quaternion.Euler(Mathf.Rad2Deg * gunRot.x, Mathf.Rad2Deg * gunRot.y, Mathf.Rad2Deg * gunRot.z) * trajVec;
                //postRotVec = Quaternion.AngleAxis(gunRot.z, Vector3.back) * trajVec;
                velocityX = postRotVec.x * velocity;
                velocityY = postRotVec.y * velocity;
                velocityZ = postRotVec.z * velocity;
                shotFired = true;
            }
            deltaTime = Time.deltaTime;
            timer -= deltaTime;
            elapsedTime += deltaTime;

            if (!dragOn)
            {
                ballPosX += (float)(velocityX * deltaTime) + (float)(1.0f / 2.0f * accelX * Mathf.Pow(deltaTime, 2));
                velocityX += (float)(accelX * deltaTime);
                ballPosY += (float)(velocityY * deltaTime) + (float)(1.0f / 2.0f * accelY * Mathf.Pow(deltaTime, 2));
                velocityY += (float)(accelY * deltaTime);
                ballPosZ += (float)(velocityZ * deltaTime) + (float)(1.0f / 2.0f * accelZ * Mathf.Pow(deltaTime, 2));
                velocityZ += (float)(accelZ * deltaTime);
            }
            else
            {
                ballPosX += (float)(velocityX * tau * (1 - Mathf.Exp(-(deltaTime / tau))) + (((windC * windVel * Mathf.Cos(((Mathf.Deg2Rad * 360) - gunRot.y))) / drag) * tau * (1 - Mathf.Exp(-(deltaTime / tau)))) - (((windC * windVel * Mathf.Cos(((Mathf.Deg2Rad * 360) - gunRot.y))) / drag) * deltaTime));
                ballPosY += (float)(velocityY * tau * (1 - Mathf.Exp(-(deltaTime / tau))) + (gravity * Mathf.Pow(tau, 2) * (1 - Mathf.Exp(-deltaTime / tau))) - (gravity * tau * deltaTime));
                ballPosZ += (float)(velocityZ * tau * (1 - Mathf.Exp(-(deltaTime / tau))) + (((windC * windVel * Mathf.Sin(((Mathf.Deg2Rad * 360) - gunRot.y))) / drag) * tau * (1 - Mathf.Exp(-(deltaTime / tau)))) - (((windC * windVel * Mathf.Sin(((Mathf.Deg2Rad * 360) - gunRot.y))) / drag) * deltaTime));
                velocityX = (float)((Mathf.Exp(-(deltaTime / tau)) * velocityX) + (Mathf.Exp(-(deltaTime / tau)) - 1) * ((windC * windVel * Mathf.Cos(((Mathf.Deg2Rad * 360) - gunRot.y))) / drag));
                velocityY = (float)((Mathf.Exp(-(deltaTime / tau)) * velocityY) + (Mathf.Exp(-(deltaTime / tau)) - 1) * gravity * tau);
                velocityZ = (float)((Mathf.Exp(-(deltaTime / tau)) * velocityZ) + (Mathf.Exp(-(deltaTime / tau)) - 1) * ((windC * windVel * Mathf.Sin(((Mathf.Deg2Rad * 360) - gunRot.y))) / drag));
            }

            /*if (((correctRotDegZ - (float)(Mathf.Rad2Deg * gunRot.z)) <= 0.1f) && ((correctRotDegY - (360.0f - (float)(Mathf.Rad2Deg * gunRot.y))) <= 0.1f))
            {
                if (ballPosX >= targetPosX)
                {
                    startTime = false;
                    shotFired = false;
                    hit = 1;
                }
            }
            else
            {
                if (ballPosX >= rangeBoundX)
                {
                    startTime = false;
                    shotFired = false;
                    hit = 2;
                }
            }*/

            if (ballPosY <= 0)
            {
                startTime = false;
                shotFired = false;

                if ((Mathf.Abs(ballPosX - targetPosX) < 20) && (Mathf.Abs(ballPosZ - targetPosZ) < 20))
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
        //CheckInput();
        correctRotDegY = (float)(Mathf.Rad2Deg * ((float)(Mathf.Acos(Mathf.Abs(targetPosX - gunPosX) / Mathf.Sqrt(Mathf.Pow(targetPosX - gunPosX, 2) + Mathf.Pow(targetPosZ - gunPosZ, 2))))));
        //correctRotDegZ = (float)(Mathf.Rad2Deg * ((float)(Mathf.Asin(2 * (((targetPosX - gunPosX) / velocity * 1.0f / 2.0f * Mathf.Abs(accelY)) / velocity)) / 2)));
        correctRotDegZ = (float)(Mathf.Rad2Deg * ((float)(Mathf.Asin(Mathf.Abs(accelY) * Mathf.Sqrt(Mathf.Pow((targetPosX - gunPosX), 2) + Mathf.Pow((targetPosZ - gunPosZ), 2)) / Mathf.Pow(velocity, 2)))) / 2);
        //Console.WriteLine(correctRotDeg);
        gunRot.y = Mathf.Deg2Rad * (360.0f - (correctRotDegY + gunRotDegY));
        gunRot.z = Mathf.Deg2Rad * ((correctRotDegZ + gunRotDegZ));

        if (frameCount > 5)
        {
            GameObject c = Instantiate(go, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
            arc.Add(c);
            frameCount = 0;
        }
        frameCount++;
        tau = ballMass / drag;

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
            velocity -= 10.0f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            velocityUp = true;
        }
        if (Input.GetKeyUp(KeyCode.X) && velocityUp)
        {
            velocityUp = false;
            velocity += 10.0f;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            gunRotZ = true;
        }
        if (Input.GetKeyUp(KeyCode.W) && gunRotZ)
        {
            gunRotZ = false;
            gunRotDegZ += Mathf.Rad2Deg * 0.02f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            gunRotZ = true;
        }
        if (Input.GetKeyUp(KeyCode.S) && gunRotZ)
        {
            gunRotZ = false;
            gunRotDegZ -= Mathf.Rad2Deg * 0.02f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            gunRotY = true;
        }
        if (Input.GetKeyUp(KeyCode.A) && gunRotY)
        {
            gunRotY = false;
            gunRotDegY += Mathf.Rad2Deg * 0.02f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            gunRotY = true;
        }
        if (Input.GetKeyUp(KeyCode.D) && gunRotY)
        {
            gunRotY = false;
            gunRotDegY -= Mathf.Rad2Deg * 0.02f;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            dragOn = !dragOn;
            timer = timerInit;
            elapsedTime = 0;
            ballPosX = ballPosXInit;
            ballPosY = ballPosYInit;
            ballPosZ = ballPosZInit;
            velocity = velocityInit;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0 * 30, 300, 300), "Projectile Position: (" + ballPosX + ", " + ballPosY + ", " + ballPosZ + ")");
        GUI.Label(new Rect(0, 1 * 30, 300, 300), "Target Position: (" + targetPosX + ", " + targetPosY + ", " + targetPosZ + ")");
        GUI.Label(new Rect(0, 2 * 30, 300, 300), "Range: (" + Mathf.Abs(ballPosX - targetPosX) + ", " + Mathf.Abs(ballPosY - targetPosY) + ", " + Mathf.Abs(ballPosZ - targetPosZ) + ")");
        //GUI.Label(new Rect(0, 3 * 30, 300, 300), "Correct Rotation: " + correctRotDegZ);
        GUI.Label(new Rect(0, 3 * 30, 300, 300), "Correct Alpha: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * (90 - correctRotDegZ)))));
        GUI.Label(new Rect(0, 4 * 30, 300, 300), "Correct Gamma: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * (correctRotDegY)))));
        GUI.Label(new Rect(0, 5 * 30, 300, 300), "Current Alpha: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * 90) - gunRot.z)));
        GUI.Label(new Rect(0, 6 * 30, 300, 300), "Current Gamma: " + (Mathf.Rad2Deg * ((Mathf.Deg2Rad * 360) - gunRot.y)));
        GUI.Label(new Rect(0, 7 * 30, 300, 300), "Time: " + elapsedTime + " s");
        GUI.Label(new Rect(0, 8 * 30, 300, 300), "Velocity: " + velocity + " m/s");
        GUI.Label(new Rect(0, 9 * 30, 300, 300), "Tau: " + tau + " s");
        if (dragOn)
        {
            if (hit == 1)
            {
                GUI.Label(new Rect(0, 10 * 30, 300, 300), "Hit or Miss with Drag and Wind: Hit");
            }
            else if (hit == 2)
            {
                GUI.Label(new Rect(0, 10 * 30, 300, 300), "Hit or Miss with Drag and Wind: Miss");
            }
        }
        else
        {
            GUI.Label(new Rect(0, 10 * 30, 300, 300), "Hit or Miss with Drag and Wind: NA");
        }
        GUI.Label(new Rect(0, 11 * 30, 300, 300), "Wind: (" + windVel + " m/s, " + (Mathf.Rad2Deg * windDir) + " degrees)");
        GUI.Label(new Rect(0, 12 * 30, 300, 300), "Projectile Mass: " + ballMass + " kg");
        GUI.Label(new Rect(0, 13 * 30, 300, 300), "Drag Constant: " + drag + " N/m/s");

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
