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
	
	void Awake()
	{
		image = GetComponent<Image>();
		rectTransform = GetComponent<RectTransform>();
	}
	
	public void Mark()
	{
		StartCoroutine(SetHero());
	}
	
	public void Fade()
	{
		StartCoroutine(FadeHeroToMarked());
	}
	
	private IEnumerator SetHero()
	{
		//set color to heroColor and scale local scale from 2 to 2.5 over 0.1 seconds
		state = PipState.Hero;
		image.color = heroColor;
		rectTransform.localScale = Vector3.one * 2.5f;
		
		float t = 0;
		while (t < 0.1f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(2.5f, 1.5f, t / 0.1f);
			yield return null;
		}
		rectTransform.localScale = Vector3.one * 1.5f;
	}
	
	private IEnumerator FadeHeroToMarked()
	{
		//set color to markedColor and scale local scale from 2 to 1 over 0.5 seconds
		state = PipState.Marked;
		image.color = markedColor;
		rectTransform.localScale = Vector3.one * 2f;
		
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(2f, 1, t / 0.5f);
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
	Danger
}
