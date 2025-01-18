using UnityEngine;

public class ProgressBar : MonoBehaviour
{
	private RectTransform bar;
	private float maxWidth;
	
	void Start()
	{
		bar = GetComponent<RectTransform>();
		maxWidth = transform.parent.GetComponent<RectTransform>().rect.width;
	}
	
	public void UpdateBar(float progress)
	{
		//set width of bar to progress * maxWidth
		bar.sizeDelta = new Vector2(progress * maxWidth, bar.sizeDelta.y);
	}
}
