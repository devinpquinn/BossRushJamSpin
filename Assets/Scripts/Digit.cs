using UnityEngine;
using TMPro;

public class Digit : MonoBehaviour
{
	private TextMeshProUGUI text;
	private int value;
	
	private void Awake()
	{
		text = GetComponentInChildren<TextMeshProUGUI>();
		value = 0;
		text.text = value.ToString();
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
	}
	
	//decrease value by 1; if value is 0, set it to 9; update text
	public void Decrease()
	{
		value--;
		if (value < 0)
			value = 9;
		text.text = value.ToString();
	}
}
