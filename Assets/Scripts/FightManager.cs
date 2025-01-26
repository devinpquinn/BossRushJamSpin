using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour
{
	public int level = 1;
	
	void Start()
	{
		StartCoroutine(Boss1_Phase1());
	}
	
	IEnumerator Boss1_Phase1()
	{
		yield return null;
	}
}
