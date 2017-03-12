using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMCredits : MonoBehaviour {

    [SerializeField]
    private Animator anim;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Item")) SceneManager.LoadScene("MainMenu");
        if (Input.GetButton("Glasses")) anim.SetTrigger("3D");
        if (Input.GetKey("left")) anim.SetTrigger("Left");
        if (Input.GetKey("right")) anim.SetTrigger("Right");

    }
    

    public void BackToMenu(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }
}
