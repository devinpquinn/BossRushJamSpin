using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
	public List<GameObject> attacks;
		//0 = VertSmallLeft
		//1 = VertSmallRight
		//2 = HorizSmallLeft
		//3 = HorizSmallRight
		//4 = VertLargeLeft
		//5 = VertLargeRight
		//6 = HorizLargeLeft
		//7 = HorizLargeRight
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
	
	public IEnumerator Attack(string attackType)
	{
		switch (attackType)
		{
			case "VertSmallLeft": //small bar left to right
				yield return ShowWarnings(false, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
				Instantiate(attacks[0], transform);
				break;
			case "VertSmallRight": //small bar right to left
				yield return ShowWarnings(true, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
				Instantiate(attacks[1], transform);
				break;
			case "HorizSmallLeft": //small bolt left to right
				int randomRow = Random.Range(0, 10);
				yield return ShowWarnings(false, new List<int> { randomRow });
				GameObject thisAttack = Instantiate(attacks[2], transform);
				thisAttack.transform.GetChild(0).position = new Vector3(thisAttack.transform.GetChild(0).position.x, warningsLeft[randomRow].transform.position.y, thisAttack.transform.GetChild(0).position.z);
				break;
			case "HorizSmallRight": //small bolt right to left
				randomRow = Random.Range(0, 10);
				yield return ShowWarnings(true, new List<int> { randomRow });
				thisAttack = Instantiate(attacks[3], transform);
				thisAttack.transform.GetChild(0).position = new Vector3(thisAttack.transform.GetChild(0).position.x, warningsRight[randomRow].transform.position.y, thisAttack.transform.GetChild(0).position.z);
				break;
			case "VertLargeLeft": //large bar left to right
				yield return ShowWarnings(false, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
				Instantiate(attacks[4], transform);
				break;
			case "VertBarRight": //large bar right to left
				yield return ShowWarnings(true, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 });
				Instantiate(attacks[5], transform);
				break;
			case "HorizLargeLeft": //large bolt left to right
				randomRow = Random.Range(1, 9);
				yield return ShowWarnings(false, new List<int> { randomRow, randomRow - 1 < 0 ? 0 : randomRow - 1, randomRow + 1 > 9 ? 9 : randomRow + 1 });
				thisAttack = Instantiate(attacks[6], transform);
				thisAttack.transform.GetChild(0).position = new Vector3(thisAttack.transform.GetChild(0).position.x, warningsLeft[randomRow].transform.position.y, thisAttack.transform.GetChild(0).position.z);
				break;
			case "HorizLargeRight": //large bolt right to left
				randomRow = Random.Range(1, 9);
				yield return ShowWarnings(true, new List<int> { randomRow, randomRow - 1 < 0 ? 0 : randomRow - 1, randomRow + 1 > 9 ? 9 : randomRow + 1 });
				thisAttack = Instantiate(attacks[7], transform);
				thisAttack.transform.GetChild(0).position = new Vector3(thisAttack.transform.GetChild(0).position.x, warningsRight[randomRow].transform.position.y, thisAttack.transform.GetChild(0).position.z);
				break;
			case "VertSmallBoth": //small bar both directions
				StartCoroutine(ShowWarnings(false, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
				StartCoroutine(ShowWarnings(true, new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }));
				yield return new WaitForSeconds(1);
				Instantiate(attacks[0], transform);
				Instantiate(attacks[1], transform);
				break;
		}
	}
}
