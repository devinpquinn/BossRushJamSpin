using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Pip : MonoBehaviour
{
	private Image image;
	private RectTransform rectTransform;
	public PipState state = PipState.Empty;
	public Color markedColor;
	public Color heroColor;
	public Color dangerColor;
	
	void Awake()
	{
		image = GetComponent<Image>();
		rectTransform = GetComponent<RectTransform>();
	}
	
	public void Mark()
	{
		StartCoroutine(SetHero());
	}
	
	public void Unmark()
	{
		StartCoroutine(FadeHeroToMarked());
	}
	
	public void SetDanger()
	{
		if(state == PipState.Empty)
		{
			state = PipState.DangerEmpty;
			image.color = dangerColor;
		}	
		else if(state == PipState.Marked)
		{
			state = PipState.DangerMarked;
			image.color = dangerColor;
		}
		else if(state == PipState.Hero)
		{
			Debug.Log("Hero is hit!");
		}	
	}
	
	public void SetSafe()
	{
		if(state == PipState.DangerEmpty)
		{
			state = PipState.Empty;
			image.color = Color.white;
		}	
		else if(state == PipState.DangerMarked)
		{
			state = PipState.Marked;
			image.color = markedColor;
		}
	}
	
	private IEnumerator SetHero()
	{
		//set color to heroColor and scale local scale from 2 to 2.5 over 0.1 seconds
		state = PipState.Hero;
		image.color = heroColor;
		rectTransform.localScale = Vector3.one * 5f;
		
		float t = 0;
		while (t < 0.1f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(5f, 3f, t / 0.1f);
			yield return null;
		}
		rectTransform.localScale = Vector3.one * 3f;
	}
	
	private IEnumerator FadeHeroToMarked()
	{
		//set color to markedColor and scale local scale from 2 to 1 over 0.5 seconds
		state = PipState.Marked;
		image.color = markedColor;
		rectTransform.localScale = Vector3.one * 2.5f;
		
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(2.5f, 1, t / 0.5f);
			yield return null;
		}
		rectTransform.localScale = Vector3.one;
	}
}

public enum PipState
{
	Empty,
	Marked,
	Hero,
	DangerEmpty,
	DangerMarked
}
