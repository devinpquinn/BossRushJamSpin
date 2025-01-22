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
	private Animator anim;
	
	private float lastScrollTime = 0f;
	private const float scrollCooldown = 0.1f;
	
	private Coroutine shakeCoroutine;
	private Vector3 shakeOffset = new Vector3(0, 5, 0);
	private Vector3 shakeRotation = new Vector3(5, 0, 0);
	private Vector3 aboveTextBaseRotation;
	private Vector3 belowTextBaseRotation;

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
		anim.Play("Digit_Select");
	}
	
	//unhighlight digit when mouse leaves it
	void OnMouseExit()
	{
		anim.Play("Digit_Deselect");
	}
	
	//if player scrolls mouse wheel while hovering over digit, increase or decrease value
	void OnMouseOver()
	{
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
		
		lockManager.CheckCode();
		
		if(shakeCoroutine != null)
			StopCoroutine(shakeCoroutine);
			
		shakeCoroutine = StartCoroutine(ShakeTextDown());
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
		
		yield return new WaitForSeconds(scrollCooldown * 0.95f);
		
		textParent.transform.localPosition = Vector3.zero;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation;
	}
	
	private IEnumerator ShakeTextDown()
	{
		textParent.transform.localPosition = Vector3.zero - shakeOffset;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation - shakeRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation + shakeRotation;
		
		yield return new WaitForSeconds(scrollCooldown * 0.95f);
		
		textParent.transform.localPosition = Vector3.zero;
		
		aboveText.transform.localEulerAngles = aboveTextBaseRotation;
		belowText.transform.localEulerAngles = belowTextBaseRotation;
	}
}
