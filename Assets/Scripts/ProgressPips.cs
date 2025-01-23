using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressPips : MonoBehaviour
{
	private List<Pip> pips;
	public GameObject pipPrefab;
	public NotchManager notchManager;
	
	public void SetupPips(int numPips)
	{
		//spawn numPips pips as children of this object
		pips = new List<Pip>();
		for (int i = 0; i < numPips; i++)
		{
			GameObject pip = Instantiate(pipPrefab, transform);
			pip.name = i.ToString();
			Pip thisPip = pip.GetComponent<Pip>();
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
		
		//set notch based on tens digit of pipIndex
		notchManager.SetNotch((pipIndex % 100) / 10);
	}
	
	public void BumpPip(int pipIndex)
	{
		pips[pipIndex].Bump();
		
		//set notch based on tens digit of pipIndex
		notchManager.SetNotch((pipIndex % 100) / 10);
	}
	
	public bool PipIsDanger(int pipIndex)
	{
		return pips[pipIndex].state == PipState.DangerEmpty || pips[pipIndex].state == PipState.DangerMarked;
	}
}
