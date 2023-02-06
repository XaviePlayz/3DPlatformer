using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : Pickup {	

    public AudioSource audioSource;

    public AudioClip healthClip;

    public int health;

	protected override void OnContactDo(){
        FPSPlayerController.FPSPlayerInstance.AddHealth(health);
        audioSource.PlayOneShot(healthClip);
        StartCoroutine(WaitForSoundToPlay());
    }

    private IEnumerator WaitForSoundToPlay(){
        YieldInstruction endOfFrame = new WaitForEndOfFrame();
        while(audioSource.isPlaying)
            yield return endOfFrame;
        Destroy(gameObject);
    }
}
