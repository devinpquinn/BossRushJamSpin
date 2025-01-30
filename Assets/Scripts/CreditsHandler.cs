using UnityEngine;
using UnityEngine.UI;

public class CreditsHandler : MonoBehaviour
{
	private Image background;
	
	//over the course of 5 seconds, cycle the hue of the background color
	void Start()
	{
		background = GetComponent<Image>();
		InvokeRepeating("CycleHue", 0, 0.1f);
	}
	
	void CycleHue()
	{
		Color.RGBToHSV(background.color, out float h, out float s, out float v);
		h += 0.01f;
		if(h > 1)
		{
			h = 0;
		}
		background.color = Color.HSVToRGB(h, s, v);
	}
}
