using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMCredits : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Item")) SceneManager.LoadScene("MainMenu");
	}
    

    public void BackToMenu(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }
}
