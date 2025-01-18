using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPips : MonoBehaviour
{
	private List<Pip> pips;
	public GameObject pipPrefab;
	
	public void SetupPips(int numPips)
	{
		//spawn numPips pips as children of this object
		pips = new List<Pip>();
		for (int i = 0; i < numPips; i++)
		{
			GameObject pip = Instantiate(pipPrefab, transform);
			pips.Add(pip.gameObject.GetComponent<Pip>());
		}
	}
	
	public void MarkPip(int pipIndex)
	{
		//change color of pip image at pipIndex to markedColor
		pips[pipIndex].Mark();
	}
}
