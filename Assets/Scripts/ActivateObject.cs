using UnityEngine;

public class ActivateObject : MonoBehaviour
{
	public GameObject target;
	
	public void Activate()
	{
		LockManager.live = true;
		target.SetActive(true);
	}
}
