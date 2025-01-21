using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI
;

public class LockManager : MonoBehaviour
{
	public static LockManager instance;
	public List<Digit> digits;
	private int secretCode = -1;
	private int minimumGuesses = 99;
	private List<int> codesTried;
	public ProgressPips progressPips;
	public ProgressBar progressBar;
	
	public Image healthBarL;
	public Image healthBarR;
	private float damage = 0;
	
	[HideInInspector] public int heroPip = -1;
	
	void Awake()
	{
		instance = this;
	}
	
	void Start()
	{
		FillDigits();
			
		//GenerateCode();
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
		
		//mark progress pip corresponding to code
		progressPips.MarkPip(code);
			
		//update progress bar
		progressBar.UpdateBar((float)codesTried.Count / Mathf.Pow(10, digits.Count));
		
		if(secretCode == -1 && codesTried.Count >= minimumGuesses)
		{
			GenerateCode();
		}
		
		if(code == secretCode)
		{
			Debug.Log(code + " is correct!");
		}
		else
		{
			//Debug.Log(code + " is incorrect.");
		}
	}
	
	public void Damage()
	{
		damage += 0.1f;
		
		healthBarL.fillAmount = Mathf.Lerp(0, 1, damage / 0.5f);
		healthBarL.transform.parent.localScale = Vector3.one * Mathf.Lerp(1, 1.5f, damage / 0.5f);
		
		healthBarR.fillAmount = Mathf.Lerp(0, 1, (damage - 0.5f) / 0.5f);
		healthBarR.transform.parent.localScale = Vector3.one * Mathf.Lerp(1, 1.5f, (damage - 0.5f) / 0.5f);
		
		if(damage >= 1)
		{
			Debug.Log("Hero destroyed!");
		}
	}
}
