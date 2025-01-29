using UnityEngine;

public class ShakeTrigger : MonoBehaviour
{
	public Shake shake;
	
	public void TriggerShake()
	{
		shake.duration = 0.5f;
		shake.start = true;
	}
}
