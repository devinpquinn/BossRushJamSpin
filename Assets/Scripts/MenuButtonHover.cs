using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButtonHover : MonoBehaviour
{
	public TextMeshProUGUI text;
	
	private Color baseColor;
	private Color hoverColor = Color.white;
	
	void Start()
	{
		baseColor = text.color;
	}
	
	public void OnHover()
	{
		text.color = hoverColor;
	}
	
	public void OnExit()
	{
		text.color = baseColor;
	}
}
