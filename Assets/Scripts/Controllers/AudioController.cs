using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AudioController : MonoBehaviour
{
    private AudioSource[] musicTracks;

    private void Awake() {
        musicTracks = gameObject.GetComponents<AudioSource>();
        Debug.Log(musicTracks.Count()); 
    }
}
