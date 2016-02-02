using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnitSelectionMenu : MonoBehaviour {
    [SerializeField]
    Button StartButton;
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
        SceneManager.LoadScene("GameLvl1");
    }
}
