using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
    float camX = 0.0f;
    float camY = 3.0f;
    float camZ = -15.0f;
    float camSpeed = 0.1f;
    float camElevateSpeed = 1.0f;
    int screenBoundPad = 10;
    float oldCamPositionX;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.GetMouseButtonDown(0))
        {
            /*RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if(Physics.Raycast(gameObject.GetComponent<UnityEngine.Camera>().main.ScreenPointToRay(Input.mousePosition), out hitInfo))
            {

            }*/
        }

        FollowMouse();

        if (Input.GetMouseButton(1))
        {
            Orbit();
        }
        oldCamPositionX = Input.mousePosition.x;
    }

    // When mouse position is at edge of camera bounds the camera position changes
    public void FollowMouse()
    {
        // When mouse hits right camera bound, move camera right
        if(Input.mousePosition.x > Screen.width - screenBoundPad)
        {
            camX = camSpeed;
        }
        // When mouse hits left camera bound, move camera left
        else if (Input.mousePosition.x < 0 + screenBoundPad)
        {
            camX = -camSpeed;
        }
        else
        {
            camX = 0;
        }

        // When mouse hits top camera bound, move camera forward
        if (Input.mousePosition.y > Screen.height - screenBoundPad)
        {
            camY = camSpeed;
        }
        // When mouse hits bottom camera bound, move camera back
        else if (Input.mousePosition.y < 0 + screenBoundPad)
        {
            camY = -camSpeed;
        }
        else
        {
            camY = 0;
        }


        // When mouse scroll forward, move camera up
        if (Input.GetAxis("Mouse ScrollWheel") > 0.0f) //scroll forward
        {
            camZ = camElevateSpeed;            
        }
        // When mouse scroll backward, move camera down
        else if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
        {
            camZ = -camElevateSpeed;            
        }
        else
        {
            camZ = 0;            
        }

        transform.Translate(camX, camY, camZ);
    }

    // Rotate camera around center of camera based on delta of mouse's x position
    public void Orbit()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, Camera.main.nearClipPlane)); //centre point of camera
        float rotation = Input.mousePosition.x - oldCamPositionX;
        transform.RotateAround(target, Vector3.up, rotation);
    }
}
