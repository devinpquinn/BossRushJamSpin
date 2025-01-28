using UnityEngine;

public class MenuController : MonoBehaviour
{
	public GameObject continueButton;
	
	void Start()
	{
		if(PlayerPrefs.HasKey("Boss") && PlayerPrefs.GetInt("Boss") > 1)
		{
			continueButton.SetActive(true);
		}
	}
}
