using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
	public void GoToMenu()
	{
		SceneManager.LoadScene("Menu");
	}
}
