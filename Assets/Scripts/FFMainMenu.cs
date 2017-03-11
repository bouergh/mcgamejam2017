using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FFMainMenu : MonoBehaviour {

	public void PlayGame(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
