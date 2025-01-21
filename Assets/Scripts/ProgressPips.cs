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
		Vector2 boxSize = GetComponent<GridLayoutGroup>().cellSize;
		pips = new List<Pip>();
		for (int i = 0; i < numPips; i++)
		{
			GameObject pip = Instantiate(pipPrefab, transform);
			pips.Add(pip.gameObject.GetComponent<Pip>());
			
			//set size of pip box collider 2d to match grid cell size
			BoxCollider2D boxCollider2D = pip.GetComponent<BoxCollider2D>();
			boxCollider2D.size = boxSize;
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
}
