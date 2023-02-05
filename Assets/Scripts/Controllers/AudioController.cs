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
        private AudioSource currentlyPlaying;
        private AudioSource upNext;

        private void Awake() {
            musicTracks = gameObject.GetComponents<AudioSource>();
        }

        private void Start() {
        }

        public void PlayMusicTrack(MusicTracks track) {
            AudioSource playing = musicTracks.Single(source => source.isPlaying);
            playing.Stop();

            int trackIndex = (int)track;
            AudioSource newTrack = musicTracks[trackIndex] as AudioSource;
            newTrack.Play();
        }

        public string GetMusicTrack(MusicTracks track) {
            if (musicTracks[(int)track]) {
                return (musicTracks[(int)track] as AudioSource).name;
            }

            return null;
        }
    }
}
