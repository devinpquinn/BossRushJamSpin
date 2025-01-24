using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using System.Security.Cryptography;

public class Pip : MonoBehaviour
{
	[HideInInspector] public Image image;
	private BoxCollider2D boxCollider;
	private Vector2 baseBoxColliderSize;
	private RectTransform rectTransform;
	public PipState state = PipState.Empty;
	private int dangerCount = 0;
	
	private Coroutine resizeCoroutine;
	
	void Awake()
	{
		image = GetComponent<Image>();
		boxCollider = GetComponent<BoxCollider2D>();
		baseBoxColliderSize = boxCollider.size;
		rectTransform = GetComponent<RectTransform>();
	}
	
	public void Mark()
	{
		if(resizeCoroutine != null)
			StopCoroutine(resizeCoroutine);
		
		if(state == PipState.DangerEmpty)
		{
			LockManager.instance.Damage();
		}
		
		state = PipState.Hero;
		image.color = LockManager.instance.heroColor;
		resizeCoroutine = StartCoroutine(LerpScale(5, 3, 0.1f));
	}
	
	public void Unmark()
	{
		if(resizeCoroutine != null)
			StopCoroutine(resizeCoroutine);
			
		
		if(dangerCount < 1)
		{
			state = PipState.Marked;
			image.color = LockManager.instance.markedColor;
		}
		else
		{
			state = PipState.DangerMarked;
			image.color = LockManager.instance.dangerColor;
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
			image.color = LockManager.instance.dangerColor;
		}	
		else if(state == PipState.Marked)
		{
			state = PipState.DangerMarked;
			image.color = LockManager.instance.dangerColor;
		}	
		
		if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
		
		resizeCoroutine = StartCoroutine(LerpScale(1, 1.75f, 0.1f));
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
			image.color = LockManager.instance.baseColor;
		}	
		else if(state == PipState.DangerMarked)
		{
			state = PipState.Marked;
			image.color = LockManager.instance.markedColor;
		}
		
		if(resizeCoroutine != null)
				StopCoroutine(resizeCoroutine);
		
		resizeCoroutine = StartCoroutine(LerpScale(1.75f, 1, 0.1f));
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
				image.color = LockManager.instance.markedColor;
			}
			else
			{
				state = PipState.DangerMarked;
				image.color = LockManager.instance.dangerColor;
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
