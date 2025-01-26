using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour
{
	public int testPhase = -1;
	private AttackHandler attackHandler;
	
	void Start()
	{
		attackHandler = GetComponent<AttackHandler>();
	}
	
	void Update()
	{
		if(testPhase != -1)
		{
			StopAllCoroutines();
			switch(testPhase)
			{
				case 1:
					StartCoroutine(Boss1_Phase1());
					break;
				case 2:
					StartCoroutine(Boss1_Phase2());
					break;
			}
			testPhase = -1;
		}
	}
	
	IEnumerator Boss1_Phase1()
	{
		yield return new WaitForSeconds(10f);
		
		//alternate HorizSmallRight and HorizSmallLeft attacks every 5 seconds
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("HorizSmallRight"));
			yield return new WaitForSeconds(5);
			yield return StartCoroutine(attackHandler.Attack("HorizSmallLeft"));
			yield return new WaitForSeconds(5);
		}
	}
	
	IEnumerator Boss1_Phase2()
	{
		yield return new WaitForSeconds(5f);
		
		//alternate VertSmallRight and VertSmallLeft attacks every 35 seconds
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("VertSmallRight"));
			yield return new WaitForSeconds(35);
			yield return StartCoroutine(attackHandler.Attack("VertSmallLeft"));
			yield return new WaitForSeconds(35);
		}
	}
}
