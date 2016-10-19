using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    Button PlayButton;
    public GameObject optionsHolder;

	// Use this for initialization
	void Start ()
    {
	    if(optionsHolder.GetComponent<OptionsHolder>().initialStart)
        {
            optionsHolder.GetComponent<OptionsHolder>().InitializeValues();
            optionsHolder.GetComponent<OptionsHolder>().initialStart = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void OnPlayClick()
    {
        DontDestroyOnLoad(optionsHolder);
        SceneManager.LoadScene("UnitSelection");
    }

    public void OnOptionsClick()
    {
        DontDestroyOnLoad(optionsHolder);
        SceneManager.LoadScene("Options");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }
}
