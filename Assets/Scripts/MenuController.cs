using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
	public GameObject continueButton;
	private Animator animator;
	
	private bool live = true;
	
	void Start()
	{
		animator = GetComponent<Animator>();
		
		if(PlayerPrefs.HasKey("Boss") && PlayerPrefs.GetInt("Boss") > 1)
		{
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
	
	IEnumerator DoQuit()
	{
		live = false;
		animator.Play("MenuAnim_Out");
		yield return new WaitForSeconds(1f);
		Application.Quit();
	}
}
