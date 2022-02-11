using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void PlaySound(int sound)
    {
        source.PlayOneShot(audioClips[sound]);
    }
}
