using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	public float timeToDestroy = 1.0f;
	
	void Start()
	{
		Destroy(gameObject, timeToDestroy);
	}
}
