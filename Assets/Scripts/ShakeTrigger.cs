using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
	public Shake shake;
	
	public void TriggerShake()
	{
		shake.start = true;
	}
}
