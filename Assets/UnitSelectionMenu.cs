using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnitSelectionMenu : MonoBehaviour {
    [SerializeField]
    Button startButton;
    [SerializeField]
    GameObject unitCounter;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartClick()
    {
        DontDestroyOnLoad(unitCounter);
        SceneManager.LoadScene("GameLvl1");
    }
}
