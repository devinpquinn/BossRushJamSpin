using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Security.Cryptography;

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
	
	private int dangerCount = 0;
	
	private Coroutine resizeCoroutine;
	
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
		
		if(resizeCoroutine != null)
			StopCoroutine(resizeCoroutine);
		
		resizeCoroutine = StartCoroutine(SetHero());
	}
	
	public void Unmark()
	{
		if(resizeCoroutine != null)
			StopCoroutine(resizeCoroutine);
			
		resizeCoroutine = StartCoroutine(FadeHeroToMarked());
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
		
		dangerCount++;
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
		
		dangerCount--;
	}
	
	public void Bump()
	{
		if(transform.GetSiblingIndex() == LockManager.instance.heroPip)
		{
			if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
				
			resizeCoroutine = StartCoroutine(BumpHero());
		}
		else
		{
			if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
				
			resizeCoroutine = StartCoroutine(FadeHeroToMarked());
		}
	}
	
	private IEnumerator SetHero()
	{
		//set color to heroColor and scale local scale
		state = PipState.Hero;
		image.color = heroColor;
		rectTransform.localScale = Vector3.one * 5f;
		boxCollider.size = baseBoxColliderSize / 5f;
		
		float t = 0;
		while (t < 0.1f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(5f, 3f, t / 0.1f);
			boxCollider.size = Vector2.Lerp(baseBoxColliderSize / 5f, baseBoxColliderSize / 3f, t / 0.1f);
			yield return null;
		}
		
		rectTransform.localScale = Vector3.one * 3f;
		boxCollider.size = baseBoxColliderSize / 3f;
	}
	
	private IEnumerator FadeHeroToMarked()
	{
		//set color to markedColor and scale local scale from 2 to 1 over 0.5 seconds
		if(state == PipState.Hero)
		{
			if(dangerCount < 1)
			{
				state = PipState.Marked;
				image.color = markedColor;
			}
			else
			{
				state = PipState.DangerMarked;
				image.color = dangerColor;
			}
		}
		
		rectTransform.localScale = Vector3.one * 2.5f;
		boxCollider.size = baseBoxColliderSize * 2.5f;
		
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(2.5f, 1, t / 0.5f);
			boxCollider.size = Vector2.Lerp(baseBoxColliderSize / 2.5f, baseBoxColliderSize, t / 0.5f);
			yield return null;
		}
		
		rectTransform.localScale = Vector3.one;
		boxCollider.size = baseBoxColliderSize;
	}
	
	private IEnumerator BumpHero()
	{
		rectTransform.localScale = Vector3.one * 4f;
		boxCollider.size = baseBoxColliderSize / 4f;
		
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(4f, 3f, t / 0.5f);
			boxCollider.size = Vector2.Lerp(baseBoxColliderSize / 4f, baseBoxColliderSize / 3f, t / 0.5f);
			yield return null;
		}
		
		rectTransform.localScale = Vector3.one * 3f;
		boxCollider.size = baseBoxColliderSize / 3f;
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
