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
			case 2: //small bolt left to right
				int randomRow = Random.Range(0, 10);
				yield return ShowWarnings(false, new List<int> { randomRow });
				GameObject thisAttack = Instantiate(attacks[2], transform);
				//set first transform child of attack to same Y position as selected warning
				thisAttack.transform.GetChild(0).position = new Vector3(thisAttack.transform.GetChild(0).position.x, warningsLeft[randomRow].transform.position.y, thisAttack.transform.GetChild(0).position.z);
				break;
			case 3: //small bolt right to left
				randomRow = Random.Range(0, 10);
				yield return ShowWarnings(true, new List<int> { randomRow });
				thisAttack = Instantiate(attacks[3], transform);
				//set first transform child of attack to same Y position as selected warning
				thisAttack.transform.GetChild(0).position = new Vector3(thisAttack.transform.GetChild(0).position.x, warningsRight[randomRow].transform.position.y, thisAttack.transform.GetChild(0).position.z);
				break;
		}
	}
	
	public IEnumerator AttackLoop()
	{
		yield return new WaitForSeconds(1);
		
		int attackType = 0;
		while (true)
		{
			yield return Attack(attackType);
			attackType = (attackType + 1) % attacks.Count;
			yield return new WaitForSeconds(1);
		}
	}
	
	void Start()
	{
		StartCoroutine(AttackLoop());
	}
}
