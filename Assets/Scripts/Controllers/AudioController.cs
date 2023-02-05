using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Audio {
    public enum MusicTracks {
        Default, 
        MainLoopOne,
        MainLoopTwo, 
        MainLoopThree, 
        GameOver
    }

    public class AudioController : MonoBehaviour {
        private AudioSource[] musicTracks;
        private AudioSource currentlyPlaying;
        private AudioSource upNext;

        private void Awake() {
            musicTracks = gameObject.GetComponents<AudioSource>();
        }

        private void Start() {
        }

        public void PlayMusicTrack(MusicTracks track) {

        }
    }
}
