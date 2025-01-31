using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioTrigger : MonoBehaviour
{
	public List<AudioClip> audioClips;
	
	public void PlayAudio(int index)
	{
		GetComponent<AudioSource>().PlayOneShot(audioClips[index]);
	}
}
