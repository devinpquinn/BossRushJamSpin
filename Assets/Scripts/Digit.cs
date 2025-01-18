using UnityEngine;
using TMPro;

public class Digit : MonoBehaviour
{
	[HideInInspector] public LockManager lockManager;
	
	private TextMeshProUGUI text;
	private TextMeshProUGUI aboveText;
	private TextMeshProUGUI belowText;
	[HideInInspector] public int value;
	
	private Color normalColor;
	public Color highlightColor;
	
	private GameObject arrows;

	private void Awake()
	{
		//text is attacked to the child named MainText
		text = transform.Find("MainText").GetComponent<TextMeshProUGUI>();
		
		//find above and below text objects
		aboveText = transform.Find("AboveText").GetComponent<TextMeshProUGUI>();
		belowText = transform.Find("BelowText").GetComponent<TextMeshProUGUI>();
		
		value = 0;
		text.text = value.ToString();
		
		arrows = transform.Find("Arrows").gameObject;
	}
	
	//highlight digit when mouse is over it
	void OnMouseEnter()
	{
		normalColor = text.color;
		text.color = highlightColor;
		
		arrows.SetActive(true);
	}
	
	//unhighlight digit when mouse leaves it
	void OnMouseExit()
	{
		text.color = normalColor;
		
		arrows.SetActive(false);
	}
	
	//if player scrolls mouse wheel while hovering over digit, increase or decrease value
	void OnMouseOver()
	{
		float scroll = Input.GetAxis("Mouse ScrollWheel");
		if (scroll > 0)
			Increase();
		else if (scroll < 0)
			Decrease();
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
	}
	
	private void UpdateAboveBelowText()
	{
		aboveText.text = (value == 0) ? "9" : (value - 1).ToString();
		belowText.text = (value == 9) ? "0" : (value + 1).ToString();
	}
}
