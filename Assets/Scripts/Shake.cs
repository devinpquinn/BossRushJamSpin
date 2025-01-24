using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
	public bool start = false;
	public float duration = 1f;
	public float power = 0.1f;
	
	public AnimationCurve curve;
	
	void Update()
	{
		if(start)
		{
			start = false;
			StartCoroutine(Shaking());
		}
	}
	
	IEnumerator Shaking()
	{
		Vector3 startPos = transform.position;
		float elapsed = 0f;
		
		while(elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float strength = curve.Evaluate(1 - (elapsed / duration));
			transform.position = startPos + Random.insideUnitSphere * strength * power;
			yield return null;
		}
		
		transform.position = startPos;
	}
}
