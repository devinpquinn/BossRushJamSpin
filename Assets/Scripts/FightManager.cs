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
				case 3:
					StartCoroutine(Boss2_Phase1());
					break;
				case 4:
					StartCoroutine(Boss2_Phase2());
					break;
				case 5:
					StartCoroutine(Boss2_Phase3());
					break;
				case 6:
					StartCoroutine(Boss3_Phase1());
					break;
				case 7:
					StartCoroutine(Boss3_Phase2());
					break;
				case 8:
					StartCoroutine(Boss3_Phase3());
					break;
				case 9:
					StartCoroutine(Boss3_Phase4());
					break;
			}
			Debug.Log("Starting phase " + testPhase);
			testPhase = -1;
		}
	}
	
	IEnumerator Boss1_Phase1()
	{
		yield return new WaitForSeconds(10);
		
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
		yield return new WaitForSeconds(5);
		
		//alternate VertSmallRight and VertSmallLeft attacks every 35 seconds
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("VertSmallRight"));
			yield return new WaitForSeconds(35);
			yield return StartCoroutine(attackHandler.Attack("VertSmallLeft"));
			yield return new WaitForSeconds(35);
		}
	}
	
	IEnumerator Boss2_Phase1()
	{
		yield return new WaitForSeconds(5);
		
		//in a loop, do the following:
		//trigger 3 HorizSmallRight attacks in a burst, each 1 second apart
		//wait 2 seconds
		//trigger 3 HorizSmallLeft attacks in a burst, each 1 second apart
		//wait 2 seconds
		//trigger a VertSmallRight attack
		//repeat loop after 5 seconds, switching side of vertical attack at end of pattern
		
		bool right = false;
		while (true)
		{
			for(int i = 0; i < 3; i++)
			{
				yield return StartCoroutine(attackHandler.Attack("HorizSmallRight"));
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(2);
			for(int i = 0; i < 3; i++)
			{
				yield return StartCoroutine(attackHandler.Attack("HorizSmallLeft"));
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(2);
			
			if(right)
			{
				yield return StartCoroutine(attackHandler.Attack("VertSmallRight"));
			}
			else
			{
				yield return StartCoroutine(attackHandler.Attack("VertSmallLeft"));
			}
			right = !right;
			yield return new WaitForSeconds(5);
		}
	}
	
	IEnumerator Boss2_Phase2()
	{
		yield return new WaitForSeconds(2);
		
		//in a loop, do the following:
		//trigger 4 HorizSmallRight attacks in a burst, each 1 second apart
		//wait 1.5 seconds
		//trigger a VertSmallRight attack
		//wait 1.5 seconds
		//trigger 4 HorizSmallLeft attacks in a burst, each 1 second apart
		//wait 1.5 seconds
		//trigger a VertSmallLeft attack
		//repeat after 4 seconds
		
		while (true)
		{
			for(int i = 0; i < 4; i++)
			{
				yield return StartCoroutine(attackHandler.Attack("HorizSmallRight"));
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(1.5f);
			yield return StartCoroutine(attackHandler.Attack("VertSmallRight"));
			yield return new WaitForSeconds(1.5f);
			for(int i = 0; i < 4; i++)
			{
				yield return StartCoroutine(attackHandler.Attack("HorizSmallLeft"));
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(1.5f);
			yield return StartCoroutine(attackHandler.Attack("VertSmallLeft"));
			yield return new WaitForSeconds(4);
		}
	}
	
	IEnumerator Boss2_Phase3()
	{
		yield return new WaitForSeconds(2);
		
		//in a loop, do the following:
		//trigger 5 HorizSmallRight attacks in a burst, each 1 second apart
		//wait 1 second
		//trigger 5 HorizSmallLeft attacks in a burst, each 1 second apart
		//wait 1 second
		//trigger a VertSmallRight attack and a VertSmallLeft attack
		//repeat after 3 seconds
		
		while (true)
		{
			for(int i = 0; i < 5; i++)
			{
				yield return StartCoroutine(attackHandler.Attack("HorizSmallRight"));
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(1);
			for(int i = 0; i < 5; i++)
			{
				yield return StartCoroutine(attackHandler.Attack("HorizSmallLeft"));
				yield return new WaitForSeconds(1);
			}
			yield return new WaitForSeconds(1);
			yield return StartCoroutine(attackHandler.Attack("VertSmallBoth"));
			yield return new WaitForSeconds(3);
		}
	}
	
	IEnumerator Boss3_Phase1()
	{
		yield return new WaitForSeconds(5);
		
		//in a loop, do the following:
		//trigger a HorizLargeRight attack
		//wait 2 seconds
		//trigger a HorizLargeLeft attack
		//wait 2 seconds
		//trigger a HorizLargeBoth attack
		//repeat after 7 seconds
		
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("HorizLargeRight"));
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(attackHandler.Attack("HorizLargeLeft"));
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(attackHandler.Attack("HorizLargeBoth"));
			yield return new WaitForSeconds(7);
		}
	}
	
	IEnumerator Boss3_Phase2()
	{
		yield return new WaitForSeconds(2);
		
		//in a loop, do the following:
		//trigger a VertLargeRight attack
		//wait 9 seconds
		//trigger a VertLargeLeft attack
		//wait 9 seconds
		//trigger a VertLargeBoth attack
		//wait 9 seconds and repeat
		
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("VertLargeRight"));
			yield return new WaitForSeconds(9);
			yield return StartCoroutine(attackHandler.Attack("VertLargeLeft"));
			yield return new WaitForSeconds(9);
			yield return StartCoroutine(attackHandler.Attack("VertLargeBoth"));
			yield return new WaitForSeconds(9);
		}
	}
	
	IEnumerator Boss3_Phase3()
	{
		yield return new WaitForSeconds(2);
		
		//in a loop, do the following:
		//trigger a StrainerLargeRight attack
		//wait 3 seconds
		//trigger a StrainerLargeLeft attack
		//wait 3 seconds
		
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("StrainerLargeRight"));
			yield return new WaitForSeconds(2.5f);
			yield return StartCoroutine(attackHandler.Attack("StrainerLargeLeft"));
			yield return new WaitForSeconds(5);
		}
	}
	
	IEnumerator Boss3_Phase4()
	{
		yield return new WaitForSeconds(2);
		
		//bring it all together
		//in a loop, do the following:
		//trigger a HorizLargeRight attack
		//wait 2 seconds
		//trigger a HorizLargeLeft attack
		//wait 2 seconds
		//trigger a HorizLargeBoth attack
		//wait 7 seconds
		//trigger a VertLargeRight attack
		//wait 9 seconds
		//trigger a VertLargeLeft attack
		//wait 9 seconds
		//trigger a VertLargeBoth attack
		//wait 9 seconds
		//trigger a StrainerLargeRight attack
		//wait 3 seconds
		//trigger a StrainerLargeLeft attack
		//wait 3 seconds
		//repeat after 5 seconds
		
		while (true)
		{
			yield return StartCoroutine(attackHandler.Attack("HorizLargeRight"));
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(attackHandler.Attack("HorizLargeLeft"));
			yield return new WaitForSeconds(2);
			yield return StartCoroutine(attackHandler.Attack("HorizLargeBoth"));
			yield return new WaitForSeconds(3);
			yield return StartCoroutine(attackHandler.Attack("VertLargeRight"));
			yield return new WaitForSeconds(5);
			yield return StartCoroutine(attackHandler.Attack("VertLargeLeft"));
			yield return new WaitForSeconds(5);
			yield return StartCoroutine(attackHandler.Attack("VertLargeBoth"));
			yield return new WaitForSeconds(5);
			yield return StartCoroutine(attackHandler.Attack("StrainerLargeRight"));
			yield return new WaitForSeconds(2.5f);
			yield return StartCoroutine(attackHandler.Attack("StrainerLargeLeft"));
			yield return new WaitForSeconds(5);
		}
	}
}
