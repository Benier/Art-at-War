using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    Button PlayButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void OnPlayClick()
    {
        SceneManager.LoadScene("UnitSelection");
    }
}
