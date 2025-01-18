using UnityEngine;
using UnityEngine.UI;

public class Pip : MonoBehaviour
{
	private Image image;
	private RectTransform rectTransform;
	public Color markedColor;
	
	void Awake()
	{
		image = GetComponent<Image>();
		rectTransform = GetComponent<RectTransform>();
	}
	
	public void Mark()
	{
		StartCoroutine(Flash());
	}
	
	private System.Collections.IEnumerator Flash()
	{
		//set color to markedColor and scale local scale from 2 to 1 over 0.5 seconds
		image.color = markedColor;
		rectTransform.localScale = Vector3.one * 2;
		
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(3, 1, t / 0.5f);
			yield return null;
		}
	}
}
