using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LockManager : MonoBehaviour
{
	public static LockManager instance;
	public static bool live = false;
	public List<Digit> digits;
	private int secretCode = -1;
	private int minimumGuesses = 1;
	private List<int> codesTried;
	public ProgressPips progressPips;
	public ProgressBar progressBar;
	
	public GameObject flash;
	public GameObject victoryScreen;
	public GameObject defeatScreen;
	
	public Image healthMeter;
	private float damage = 1;
	
	public Shake shake;
	
	private bool invulnerable = false;
	public Color baseColor;
	public Color markedColor;
	public Color heroColor;
	private Color trueHeroColor;
	public Color dangerColor;
	
	[HideInInspector] public int heroPip = -1;
	
	void Awake()
	{
		instance = this;
		
		trueHeroColor = heroColor;
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
		
		//set live
		live = true;
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
		if(!live)
		{
			return;
		}
		
		string codeString = "";
		foreach (Digit digit in digits)
			codeString += digit.value.ToString();
			
		//construct int from string
		int code = int.Parse(codeString);
		
		//if code has already been tried, return
		if (codesTried.Contains(code))
		{
			progressPips.BumpPip(code);
			return;
		}
			
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
			live = false;
			
			StartCoroutine(Victory());
		}
		else
		{
			//Debug.Log(code + " is incorrect.");
		}
	}
	
	public void Damage()
	{
		if(!live || invulnerable)
		{
			return;
		}
		
		damage += 0.1f;
		
		healthMeter.fillAmount = Mathf.Lerp(0, 1, damage / 1f);
		healthMeter.transform.parent.localScale = Vector3.one * Mathf.Lerp(1, 2f, damage / 1f);
		
		if(damage > 1.01f)
		{
			Debug.Log("Hero destroyed!");
			live = false;
			
			StartCoroutine(Defeat());
		}
		else
		{
			StartCoroutine(BlinkHero());
		}
	}
	
	private IEnumerator BlinkHero()
	{
		//set hero invulnerable
		invulnerable = true;
		
		//shake
		shake.start = true;
		
		//every 0.1 seconds for 1 second, toggle hero pip color between trueHeroColor and white, also setting heroColor to white when hero pip is white
		float t = 0;
		while (t < 1)
		{
			t += Time.deltaTime;
			if (t % 0.1f < Time.deltaTime)
			{
				progressPips.pips[heroPip].image.color = progressPips.pips[heroPip].image.color == trueHeroColor ? Color.white : trueHeroColor;
				heroColor = progressPips.pips[heroPip].image.color;
			}
			yield return null;
		}
		
		//restore hero color
		progressPips.pips[heroPip].image.color = trueHeroColor;
		heroColor = trueHeroColor;
		
		//set hero vulnerable
		invulnerable = false;
	}
	
	private IEnumerator Victory()
	{
		//for each digit, disable arrows
		foreach (Digit digit in digits)
		{
			digit.anim.Play("Digit_Victory");
		}
		
		//setup hero pip transform
		progressPips.gameObject.GetComponent<GridLayoutGroup>().enabled = false;
		Transform heroPipTransform = progressPips.pips[heroPip].transform;
		heroPipTransform.parent = heroPipTransform.parent.parent;
		
		//set to third-to-last sibling
		heroPipTransform.SetAsLastSibling();
		
		//flash
		flash.GetComponent<Image>().color = heroColor;
		flash.SetActive(true);
		
		//shake
		shake.duration  = 1;
		shake.start = true;
		
		//lerp hero pip to scale 1000 over 1 second
		float t = 0;
		while (t < 1)
		{
			t += Time.deltaTime;
			float easedT = t * t * (3f - 2f * t); // Quadratic ease-in-out
			heroPipTransform.localScale = Vector3.one * Mathf.Lerp(1, 1000, easedT);
			yield return null;
		}
		heroPipTransform.localScale = Vector3.one * 1000;
		
		victoryScreen.SetActive(true);
	}
	
	private IEnumerator Defeat()
	{
		foreach (Digit digit in digits)
		{
			digit.anim.Play("Digit_Defeat");
		}
		
		//setup hero pip transform
		progressPips.gameObject.GetComponent<GridLayoutGroup>().enabled = false;
		Transform heroPipTransform = progressPips.pips[heroPip].transform;
		heroPipTransform.parent = heroPipTransform.parent.parent;
		
		//set to third-to-last sibling
		heroPipTransform.SetAsLastSibling();
		
		yield return new WaitForEndOfFrame();
		
		//set hero pip color to danger color
		progressPips.pips[heroPip].image.color = dangerColor;
		
		//flash
		flash.GetComponent<Image>().color = dangerColor;
		flash.SetActive(true);
		
		//shake
		shake.duration = 1;
		shake.start = true;
		
		//lerp hero pip to scale 1000 over 1 second
		float t = 0;
		while (t < 1)
		{
			t += Time.deltaTime;
			float easedT = t * t * (3f - 2f * t); // Quadratic ease-in-out
			heroPipTransform.localScale = Vector3.one * Mathf.Lerp(1, 1000, easedT);
			yield return null;
		}
		heroPipTransform.localScale = Vector3.one * 1000;
		
		defeatScreen.SetActive(true);
	}
}
