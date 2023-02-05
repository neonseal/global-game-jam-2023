using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Audio {
    public enum MusicTracks {
        Serene,
        Mild, 
        Stress, 
        GameOver
    }

    public class AudioController : MonoBehaviour {
        private static AudioSource[] musicTracks;
        private AudioSource currentlyPlaying, upNext;

        private void Awake() {
            musicTracks = gameObject.GetComponents<AudioSource>();
        }

        private void Start() {
            currentlyPlaying = GetCurrentlyPlaying();
        }

        private AudioSource GetCurrentlyPlaying() {
            return musicTracks.Single(source => source.isPlaying);
        }

        public void SwapMusicTracks(MusicTracks track) {
            StopAllCoroutines();
            int trackIndex = (int)track;

            upNext = musicTracks[trackIndex];
            StartCoroutine("FadeTracks");   
        }

        private IEnumerator FadeTracks() {
            float timeToFade = 2f;
            float timeElapsed = 0.0f;

            upNext.Play();
            while (timeElapsed < timeToFade) {
                currentlyPlaying.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                upNext.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            currentlyPlaying.Stop();
            currentlyPlaying = upNext;
        }

    }
}
