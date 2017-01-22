using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
	{
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
		SceneManager.LoadScene(sceneIndex);

	}	
	public void ClickExit()
	{
		Application.Quit();
	}

}
