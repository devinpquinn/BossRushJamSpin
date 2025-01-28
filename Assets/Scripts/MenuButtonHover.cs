using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MenuButtonHover : MonoBehaviour
{
	public TextMeshProUGUI text;
	
	private RectTransform rectTransform;
	private Coroutine hoverCoroutine;
	
	private Color baseColor;
	private Color hoverColor = Color.white;
	
	void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		baseColor = text.color;
	}
	
	public void OnHover()
	{
		if(hoverCoroutine != null)
			StopCoroutine(hoverCoroutine);
		hoverCoroutine = StartCoroutine(Hover());
	}
	
	public void OnExit()
	{
		if(hoverCoroutine != null)
			StopCoroutine(hoverCoroutine);
		hoverCoroutine = StartCoroutine(Exit());
	}
	
	IEnumerator Hover()
	{
		text.color = hoverColor;
		
		//lerp scale to 1.1 over 0.1 seconds
		float t = 0;
		while (t < 0.1f)
		{
			t += Time.unscaledDeltaTime;
			rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.1f, t / 0.1f);
			yield return null;
		}
	}
	
	IEnumerator Exit()
	{
		text.color = baseColor;
		
		//lerp scale to 1 over 0.1 seconds
		float t = 0;
		while (t < 0.1f)
		{
			t += Time.unscaledDeltaTime;
			rectTransform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, t / 0.1f);
			yield return null;
		}
	}
}
