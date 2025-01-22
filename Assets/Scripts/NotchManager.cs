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
			}
		}
		
		activeNotch = notches[notchIndex];
		
		foreach (Image image in activeNotch.images)
		{
			image.color = emphasisColor;
		}
	}
	
	
}

[System.Serializable]
public class Notch
{
	public List<Image> images;
}
