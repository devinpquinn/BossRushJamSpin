using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour
{
	public List<Digit> digits;
	
	private int secretCode;
	
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
		
		if(code == secretCode)
		{
			Debug.Log(code + " is correct!");
		}
		else
		{
			Debug.Log(code + " is incorrect.");
		}
	}
}
