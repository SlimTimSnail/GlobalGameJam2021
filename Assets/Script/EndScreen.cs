using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void OnRestartClick()
    {
        Invoke("StartGame", 0.4f);
    }

    private void StartGame()
	{
		SceneManager.LoadScene("Scene_Final");
	}

    public void OnExitClick()
	{
		Invoke("ExitGame", 0.6f);
	}

    private void ExitGame()
	{
		Application.Quit();

    #if UNITY_EDITOR
		Debug.LogWarning("Would quit in actual game executable");
    #endif
	}
}
