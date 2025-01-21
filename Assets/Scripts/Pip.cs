using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class Pip : MonoBehaviour
{
	private Image image;
	private BoxCollider2D boxCollider;
	private Vector2 baseBoxColliderSize;
	private RectTransform rectTransform;
	public PipState state = PipState.Empty;
	private Color originalColor;
	public Color markedColor;
	public Color heroColor;
	public Color dangerColor;
	
	void Awake()
	{
		image = GetComponent<Image>();
		boxCollider = GetComponent<BoxCollider2D>();
		baseBoxColliderSize = boxCollider.size;
		originalColor = image.color;
		rectTransform = GetComponent<RectTransform>();
	}
	
	public void Mark()
	{
		if(state == PipState.DangerEmpty)
		{
			LockManager.instance.Damage();
		}
		
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
			LockManager.instance.Damage();
		}	
	}
	
	public void SetSafe()
	{
		if(state == PipState.DangerEmpty)
		{
			state = PipState.Empty;
			image.color = originalColor;
		}	
		else if(state == PipState.DangerMarked)
		{
			state = PipState.Marked;
			image.color = markedColor;
		}
	}
	
	public void Bump()
	{
		StartCoroutine(FadeHeroToMarked());
	}
	
	private IEnumerator SetHero()
	{
		//set color to heroColor and scale local scale from 2 to 2.5 over 0.1 seconds
		state = PipState.Hero;
		image.color = heroColor;
		rectTransform.localScale = Vector3.one * 5f;
		
		//disable box collider
		boxCollider.enabled = false;
		
		float t = 0;
		while (t < 0.1f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(5f, 3f, t / 0.1f);
			yield return null;
		}
		
		rectTransform.localScale = Vector3.one * 3f;
		
		boxCollider.enabled = true;
		boxCollider.size = baseBoxColliderSize / 3f;
	}
	
	private IEnumerator FadeHeroToMarked()
	{
		//set color to markedColor and scale local scale from 2 to 1 over 0.5 seconds
		state = PipState.Marked;
		image.color = markedColor;
		rectTransform.localScale = Vector3.one * 2.5f;
		
		//disable box collider
		boxCollider.enabled = false;
		
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(2.5f, 1, t / 0.5f);
			yield return null;
		}
		
		rectTransform.localScale = Vector3.one;
		
		boxCollider.enabled = true;
		boxCollider.size = baseBoxColliderSize;
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.CompareTag("Danger"))
		{
			SetDanger();
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.CompareTag("Danger"))
		{
			SetSafe();
		}
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
