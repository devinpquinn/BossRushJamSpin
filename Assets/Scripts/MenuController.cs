using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	public GameObject continueButton;
	public GameObject bonusButton;
	private Animator animator;
	
	public Animator spin;
	private bool spinning = false;
	public Color victoryColor;
	
	private bool live = true;
	
	void Start()
	{
		animator = GetComponent<Animator>();
		
		if(PlayerPrefs.HasKey("Boss") && PlayerPrefs.GetInt("Boss") > 1)
		{
			if(PlayerPrefs.GetInt("Boss") > 3)
			{
				bonusButton.SetActive(true);
				
				Camera.main.backgroundColor = victoryColor;
			}
			continueButton.SetActive(true);
		}
	}
	
	public void Play()
	{
		if(!live)
		{
			return;
		}
		
		PlayerPrefs.DeleteKey("Boss");
		StartCoroutine(LoadScene(1));
	}
	
	IEnumerator LoadScene(int scene)
	{
		live = false;
		animator.Play("MenuAnim_Out");
		yield return new WaitForSeconds(1f);
		UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
	}
	
	public void Continue()
	{
		if(!live)
		{
			return;
		}
		
		StartCoroutine(LoadScene(1));
	}
	
	public void Quit()
	{
		if(!live)
		{
			return;
		}
		
		StartCoroutine(DoQuit());
	}
	
	public void Bonus()
	{
		if(!live || spinning)
		{
			return;
		}
		
		StartCoroutine(DoBonus());
	}
	
	IEnumerator DoBonus()
	{
		live = false;
		spinning = true;
		
		spin.Play("MainMenuCam_Spin", 0, 0);
		yield return new WaitForSeconds(2f);
		
		live = true;
		spinning = false;
	}
	
	IEnumerator DoQuit()
	{
		live = false;
		animator.Play("MenuAnim_Out");
		yield return new WaitForSeconds(1f);
		Application.Quit();
	}
}
