using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{

	public void LoadLevel(string level)
	{
		SceneManager.LoadScene (level);
	}

	public void QuitGame ()
	{
		Application.Quit();
	}
}
