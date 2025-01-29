using UnityEngine;

public class FadeTrigger : MonoBehaviour
{
	public Animator fade;
	
	public void FadeOut()
	{
		fade.Play("SceneFade_Out");
	}
}
