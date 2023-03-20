using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    public static AudioPool Instance;

    public List<AudioSource> sources = new List<AudioSource>();

    private int index = 0;

    private void Start()
    {
        Instance = this;
        sources.AddRange(GetComponents<AudioSource>());
    }

    public void PlayAudio(AudioClip clip)
    {
        sources[index].clip = clip;
        sources[index].Play();

        index++;
        if (index >= sources.Count)
        {
            index = 0;
        }
    }
}
