using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPips : MonoBehaviour
{
	private List<Pip> pips;
	public GameObject pipPrefab;
	
	public Color altOriginalColor;
	
	public void SetupPips(int numPips)
	{
		//spawn numPips pips as children of this object
		pips = new List<Pip>();
		for (int i = 0; i < numPips; i++)
		{
			GameObject pip = Instantiate(pipPrefab, transform);
			Pip thisPip = pip.GetComponent<Pip>();
			if ((i / 10) % 2 == 1)
			{
				thisPip.originalColor = altOriginalColor;
				thisPip.GetComponent<Image>().color = altOriginalColor;
			}
			pips.Add(thisPip);
		}
	}
	
	public void MarkPip(int pipIndex)
	{
		//set former hero pip to fade and set new hero pip to hero
		if (LockManager.instance.heroPip != -1)
			pips[LockManager.instance.heroPip].Unmark();
			
		LockManager.instance.heroPip = pipIndex;
		pips[pipIndex].Mark();
	}
	
	public void BumpPip(int pipIndex)
	{
		pips[pipIndex].Bump();
	}
	
	public bool PipIsDanger(int pipIndex)
	{
		return pips[pipIndex].state == PipState.DangerEmpty || pips[pipIndex].state == PipState.DangerMarked;
	}
}
