using UnityEngine;

public class ActivateObject : MonoBehaviour
{
	public GameObject target;
	
	public void Activate()
	{
		target.SetActive(true);
	}
}
