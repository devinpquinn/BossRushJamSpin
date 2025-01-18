using System.Collections.Generic;
using UnityEngine;

public class ProgressPips : MonoBehaviour
{
	private List<GameObject> pips;
	public GameObject pipPrefab;
	
	public void SetupPips(int numPips)
	{
		//spawn numPips pips as children of this object
		pips = new List<GameObject>();
		for (int i = 0; i < numPips; i++)
		{
			GameObject pip = Instantiate(pipPrefab, transform);
			pips.Add(pip);
		}
	}
}
