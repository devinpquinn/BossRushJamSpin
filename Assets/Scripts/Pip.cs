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
		
		state = PipState.Hero;
		image.color = heroColor;
		resizeCoroutine = StartCoroutine(LerpScale(5, 3, 0.1f));
	}
	
	public void Unmark()
	{
		if(resizeCoroutine != null)
			StopCoroutine(resizeCoroutine);
			
		
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
		resizeCoroutine = StartCoroutine(LerpScale(2.5f, 1, 0.5f));
	}
	
	public void SetDanger()
	{
		dangerCount++;
		
		if(dangerCount > 1)
		{
			if(state == PipState.Hero)
			{
				LockManager.instance.Damage();
			}
			return;
		}
		
		if(state == PipState.Hero)
		{
			LockManager.instance.Damage();
			return;
		}
		
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
		
		if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
		
		resizeCoroutine = StartCoroutine(LerpScale(1, 1.5f, 0.1f));
	}
	
	public void SetSafe()
	{
		dangerCount--;
		if(dangerCount > 0 || state == PipState.Hero)
		{
			return;
		}
		
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
		
		if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
		
		resizeCoroutine = StartCoroutine(LerpScale(1.5f, 1, 0.1f));
	}
	
	public void Bump()
	{
		if(transform.GetSiblingIndex() == LockManager.instance.heroPip)
		{
			if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
				
			resizeCoroutine = StartCoroutine(LerpScale(4, 3, 0.5f));
		}
		else
		{
			if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
				
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
			resizeCoroutine = StartCoroutine(LerpScale(2.5f, 1, 0.5f));
		}
	}
	
	private IEnumerator LerpScale(float startScale, float endScale, float duration)
	{
		rectTransform.localScale = Vector3.one * startScale;
		boxCollider.size = baseBoxColliderSize / startScale;
		
		float t = 0;
		while (t < duration)
		{
			t += Time.deltaTime;
			rectTransform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t / duration);
			boxCollider.size = Vector2.Lerp(baseBoxColliderSize / startScale, baseBoxColliderSize / endScale, t / duration);
			yield return null;
		}
		
		
		rectTransform.localScale = Vector3.one * endScale;
		boxCollider.size = baseBoxColliderSize / endScale;
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
