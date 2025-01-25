using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
	public List<GameObject> attacks;
	public List<GameObject> warningsLeft;
	public List<GameObject> warningsRight;
	
	//coroutine that takes a bool for right/left and a list of ints, toggles the selected warnings on and waits for a second
	public IEnumerator ShowWarnings(bool right, List<int> warnings)
	{
		List<GameObject> warningList = right ? warningsRight : warningsLeft;
		foreach (int i in warnings)
		{
			warningList[i].SetActive(false);
			warningList[i].SetActive(true);
		}
		yield return new WaitForSeconds(1);
	}
	
	public IEnumerator Attack(int attackType)
	{
		switch (attackType)
		{
			case 0: //vertical bar left to right
				yield return ShowWarnings(false, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
				Instantiate(attacks[0], transform);
				break;
			case 1: //vertical bar right to left
				yield return ShowWarnings(true, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
				Instantiate(attacks[1], transform);
				break;
		}
	}
	
	public IEnumerator AttackLoop()
	{
		yield return new WaitForSeconds(5);
		
		int attackType = 0;
		while (true)
		{
			yield return Attack(attackType);
			attackType = (attackType + 1) % attacks.Count;
			yield return new WaitForSeconds(5);
		}
	}
	
	void Start()
	{
		StartCoroutine(AttackLoop());
	}
}
