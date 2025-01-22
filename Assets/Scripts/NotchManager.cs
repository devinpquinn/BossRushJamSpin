using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotchManager : MonoBehaviour
{
	public Color normalColor, emphasisColor;
	
	private Notch activeNotch;
	public List<Notch> notches;
	
	//take an int 0-9 and set the corresponding notch to emphasis color after de-emphasizing the previous notch
	public void SetNotch(int notchIndex)
	{
		if(activeNotch != null)
		{
			foreach (Image image in activeNotch.images)
			{
				image.color = normalColor;
				image.transform.localScale = Vector3.one;
			}
		}
		
		activeNotch = notches[notchIndex];
		
		foreach (Image image in activeNotch.images)
		{
			image.color = emphasisColor;
			image.transform.localScale = new Vector3(1, 1.25f, 1);
		}
	}
	
	
}

[System.Serializable]
public class Notch
{
	public List<Image> images;
}
