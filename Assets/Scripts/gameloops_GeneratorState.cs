
//Assign an AudioSource to a GameObject and attach an Audio Clip in the Audio Source. Attach this script to the GameObject.

using UnityEngine;

public class gameloops_GeneratorState : MonoBehaviour
{
    // these are the musics that are going to be played for each amount of lives left
    // 1 life = gameloop_onelife, 2 lives = gameloop_twolives
    // life means amount of resources in boolean value
    AudioSource gameloop_threelives;
    AudioSource gameloop_twolives;
    AudioSource gameloop_onelife;
    AudioSource gameover_music;

    [SerializeField] private int livesLeft = 3;

    //Playing music
    bool m_Play;

    //Detect when you use the toggle, ensures music isn’t played multiple times
    bool m_ToggleChange;

    void Start()
    {
        //Fetch the AudioSource from the GameObject
        gameloop_audios = GetComponents<AudioSource>();
        gameloop_threelives = gameloop_audios[0];
        gameloop_twolives = gameloop_audios[1];
        gameloop_onelife = gameloop_audios[2];
        gameover_music = gameloop_audios[3];
    }

    void Update()
    {
        //Check to see if you just set the toggle to positive
        if (m_Play == true && m_ToggleChange == true)
        {
            playMusic(lives);
        }
        //Check if you just set the toggle to false
        if (m_Play == false && m_ToggleChange == true)
        {
            //Stop the audio
            stopAnyMusic();
            //Ensure audio doesn’t play more than once
            m_ToggleChange = false;
        }
    }

    void stopAnyMusic()
    {
        gameloop_threelives.Stop();
        gameloop_twolives.Stop();
        gameloop_onelife.Stop();
        gameover_music.Stop();
    }

    void playMusic()
    {
        if(lives == 3)
        {
            gameloop_threelives.Play();
        }
        elif(lives == 2)
        {
            gameloop_twolives.Play();
        }
        elif(lives == 1)
        {
            gameloop_onelife.Play();
        }
        elif(lives <= 0)
        {
            gameover_music.Play();
        }
    }
}
