using UnityEngine;
using TMPro;
using System.Collections;

public class Digit : MonoBehaviour
{
	[HideInInspector] public LockManager lockManager;
	
	private TextMeshProUGUI text;
	private Transform textParent;
	private TextMeshProUGUI aboveText;
	private TextMeshProUGUI belowText;
	[HideInInspector] public int value;
	[HideInInspector] public Animator anim;
	
	private float lastScrollTime = 0f;
	private const float scrollCooldown = 0.07f;
	
	private Coroutine shakeCoroutine;
	private Vector3 shakeOffset = new Vector3(0, 5, 0);
	private Vector3 shakeRotation = new Vector3(5, 0, 0);
	private Vector3 aboveTextBaseRotation;
	private Vector3 belowTextBaseRotation;
	
	public AudioSource pipSource;
	public AudioClip markSound;
	public AudioClip bumpSound;
	
	public Texture2D defaultCursor;
	public Texture2D hoverCursor;

	private void Awake()
	{
		//text is attacked to the child named MainText
		text = transform.Find("Texts").Find("MainText").GetComponent<TextMeshProUGUI>();
		textParent = text.transform.parent;
		
		//find above and below text objects
		aboveText = transform.Find("Texts").Find("AboveText").GetComponent<TextMeshProUGUI>();
		belowText = transform.Find("Texts").Find("BelowText").GetComponent<TextMeshProUGUI>();

		aboveTextBaseRotation = aboveText.transform.localEulerAngles;
		belowTextBaseRotation = belowText.transform.localEulerAngles;
		
		value = 0;
		text.text = value.ToString();
		
		anim = GetComponent<Animator>();
	}
	
	//highlight digit when mouse is over it
	void OnMouseEnter()
	{
		if(!LockManager.live)
		{
			return;
		}
			
		anim.Play("Digit_Select");
		
		//set cursor
		Cursor.SetCursor(hoverCursor, Vector2.zero, CursorMode.Auto);
	}
	
	//unhighlight digit when mouse leaves it
	void OnMouseExit()
	{
		if(!LockManager.live)
		{
			return;
		}
		
		anim.Play("Digit_Deselect");
		
		//set cursor
		Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
	}
	
	//if player scrolls mouse wheel while hovering over digit, increase or decrease value
	void OnMouseOver()
	{
		if(!LockManager.live)
		{
			return;
		}
			
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll != 0 && Time.time - lastScrollTime >= scrollCooldown)
		{
			if (scroll < 0)
			{
				Increase();
				lastScrollTime = Time.time;
			}
			else if (scroll > 0)
			{
				Decrease();
				lastScrollTime = Time.time;
			}
		}
	}
	
	//increase value by 1; if value is 9, set it to 0; update text
	public void Increase()
	{
		value++;
		if (value > 9)
			value = 0;
		text.text = value.ToString();
		
		PlaySound();
		
		UpdateAboveBelowText();
		
		lockManager.CheckCode();
		
		if(shakeCoroutine != null)
			StopCoroutine(shakeCoroutine);
			
		shakeCoroutine = StartCoroutine(ShakeTextUp());
	}
	
	//decrease value by 1; if value is 0, set it to 9; update text
	public void Decrease()
	{
		value--;
		if (value < 0)
			value = 9;
		text.text = value.ToString();
		
		UpdateAboveBelowText();
		
		PlaySound();
		
		lockManager.CheckCode();
		
		if(shakeCoroutine != null)
			StopCoroutine(shakeCoroutine);
			
		shakeCoroutine = StartCoroutine(ShakeTextDown());
	}
	
	public void PlaySound()
	{
		pipSource.pitch = Mathf.Lerp(0.8f, 1.2f, (float)value / 9f);
		
		//set pip source stereo pan based on GetCurrentCode
		pipSource.panStereo = Mathf.Lerp(-0.5f, 0.5f, ((float)lockManager.GetCurrentCode() % 100) / 99f);
		
		if(!lockManager.IsCodeTried())
		{
			pipSource.PlayOneShot(markSound);
		}
		else
		{
			pipSource.PlayOneShot(bumpSound);
		}
	}
	
	private void UpdateAboveBelowText()
	{
		aboveText.text = (value == 0) ? "9" : (value - 1).ToString();
		belowText.text = (value == 9) ? "0" : (value + 1).ToString();
	}
	
	private IEnumerator ShakeTextUp()
	{
		textParent.transform.localPosition = Vector3.zero + shakeOffset;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation + shakeRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation - shakeRotation;
		
		yield return new WaitForSeconds(scrollCooldown);
		
		textParent.transform.localPosition = Vector3.zero;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation;
	}
	
	private IEnumerator ShakeTextDown()
	{
		textParent.transform.localPosition = Vector3.zero - shakeOffset;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation - shakeRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation + shakeRotation;
		
		yield return new WaitForSeconds(scrollCooldown);
		
		textParent.transform.localPosition = Vector3.zero;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation;
	}
}
