using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
	public List<Digit> digits;
	
	private int secretCode;
	private List<int> codesTried;
	
	public ProgressPips progressPips;
	
	void Start()
	{
		FillDigits();
			
		GenerateCode();
	}
	
	void FillDigits()
	{
		//fill digits list with digits found in children
		digits = new List<Digit>();
		foreach (Transform child in transform)
		{
			Digit digit = child.GetComponent<Digit>();
			if (digit != null)
				digits.Add(digit);
		}
		
		foreach (Digit digit in digits)
			digit.lockManager = this;
			
		//setup codesTried list
		codesTried = new List<int>();
		
		//set up progress pips based on number of possible combinations of digits
		progressPips.SetupPips((int)Mathf.Pow(10, digits.Count));
	}
	
	void GenerateCode()
	{
		//generate random code based on number of digits
		secretCode = 0;
		for (int i = 0; i < digits.Count; i++)
			secretCode = secretCode * 10 + Random.Range(0, 10);
			
		Debug.Log("secret code: " + secretCode);
	}
	
	public void CheckCode()
	{
		string codeString = "";
		foreach (Digit digit in digits)
			codeString += digit.value.ToString();
			
		//construct int from string
		int code = int.Parse(codeString);
		
		//if code has already been tried, return
		if (codesTried.Contains(code))
			return;
			
		//add code to list of tried codes
		codesTried.Add(code);
		
		if(code == secretCode)
		{
			Debug.Log(code + " is correct!");
		}
		else
		{
			//Debug.Log(code + " is incorrect.");
			//mark progress pip corresponding to code
			progressPips.MarkPip(code);
		}
	}
}
