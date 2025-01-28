using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LockManager : MonoBehaviour
{
	public static LockManager instance;
	public static bool live = false;
	public List<Digit> digits;
	private int secretCode = -1;
	private List<int> codesTried;
	public ProgressPips progressPips;
	public ProgressBar progressBar;
	
	public GameObject flash;
	public GameObject victoryScreen;
	public GameObject defeatScreen;
	
	public Image healthMeter;
	private float damage = 0;
	private Coroutine damageCoroutine;
	
	public Shake shake;
	
	private bool invulnerable = false;
	public Color baseColor;
	public Color markedColor;
	public Color heroColor;
	private Color trueHeroColor;
	public Color dangerColor;
	
	[HideInInspector] public int heroPip = -1;
	
	public FightManager fightManager;
	private int Boss1_Phase2 = 225;
	private int Boss1_Solved = 450;
	
	private int Boss2_Phase2 = 200;
	private int Boss2_Phase3 = 375;
	private int Boss2_Solved = 550;
	
	private int Boss3_Phase2 = 175;
	private int Boss3_Phase3 = 350;
	private int Boss3_Phase4 = 525;
	private int Boss3_Solved = 700;
	
	private int boss = 1;
	
	public TextMeshProUGUI bossText;
	
	public GameObject pauseScreen;
	
	void Awake()
	{
		instance = this;
		
		trueHeroColor = heroColor;
	}
	
	void Start()
	{
		FillDigits();
		
		if(PlayerPrefs.HasKey("Boss"))
		{
			boss = PlayerPrefs.GetInt("Boss");
		}
		
		if(boss == 2)
		{
			bossText.text = "++";
		}
		else if(boss == 3)
		{
			bossText.text = "+++";
		}
		
		//start the fight
		fightManager.StartFight(boss);
	}
	
	public void Pause()
	{
		if(!live)
		{
			return;
		}
		
		pauseScreen.SetActive(true);
		Time.timeScale = 0;
		live = false;
	}
	
	public void Resume()
	{
		pauseScreen.SetActive(false);
		Time.timeScale = 1;
		live = true;
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
		//generate random code from 0 to 999, inclusive, that is not in codesTried
		do
		{
			secretCode = Random.Range(0, 1000);
		} while (codesTried.Contains(secretCode));
			
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
		int maxGuesses = Boss1_Solved;
		if(boss == 2)
		{
			maxGuesses = Boss2_Solved;
		}
		else if(boss == 3)
		{
			maxGuesses = Boss3_Solved;
		}
		progressBar.UpdateBar(Mathf.Clamp((float)codesTried.Count / maxGuesses, 0f, 1f));
		
		//check thresholds
		if(boss == 1)
		{
			if(codesTried.Count == Boss1_Phase2)
			{
				fightManager.SetPhase(2);
			}
			else if(codesTried.Count == Boss1_Solved)
			{
				StartCoroutine(Victory());
				
				//save boss
				PlayerPrefs.SetInt("Boss", 2);
			}
		}
		else if(boss == 2)
		{
			if(codesTried.Count == Boss2_Phase2)
			{
				fightManager.SetPhase(2);
			}
			else if(codesTried.Count == Boss2_Phase3)
			{
				fightManager.SetPhase(3);
			}
			else if(codesTried.Count == Boss2_Solved)
			{
				StartCoroutine(Victory());
				
				//save boss
				PlayerPrefs.SetInt("Boss", 3);
			}
		}
		else if(boss == 3)
		{
			if(codesTried.Count == Boss3_Phase2)
			{
				fightManager.SetPhase(2);
			}
			else if(codesTried.Count == Boss3_Phase3)
			{
				fightManager.SetPhase(3);
			}
			else if(codesTried.Count == Boss3_Phase4)
			{
				fightManager.SetPhase(4);
			}
			else if(codesTried.Count == Boss3_Solved)
			{
				StartCoroutine(Victory());
			}
		}
	}
	
	public void Damage()
	{
		if(!live || invulnerable)
		{
			return;
		}
		
		damage += 0.1f;
		
		if(damageCoroutine != null)
		{
			StopCoroutine(damageCoroutine);
		}
		damageCoroutine = StartCoroutine(DamageMeter());
		
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
	
	private IEnumerator DamageMeter()
	{
		healthMeter.transform.parent.localScale = Vector3.one * Mathf.Lerp(1, 2f, damage / 1f) * 1.1f;
		healthMeter.fillAmount = 1;
		healthMeter.color = Color.white;
		
		yield return new WaitForSeconds(0.5f);
		
		healthMeter.transform.parent.localScale = Vector3.one * Mathf.Lerp(1, 2f, damage / 1f);
		healthMeter.fillAmount = Mathf.Lerp(0, 1, damage / 1f);
		healthMeter.color = Color.red;
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
		if(live)
		{
			invulnerable = false;
		}
	}
	
	private IEnumerator Victory()
	{
		live = false;
		invulnerable = true;
		
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
		shake.duration = 1;
		shake.start = true;
		
		yield return new WaitForSeconds(0.15f);
		
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
		
		yield return new WaitForSeconds(0.15f);
		
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
