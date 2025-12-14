using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Manages all the audio in the game including the title screen, background,
/// and all sound effects.
/// </summary>
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;

    public AudioSource titleMusic;

    public List<AudioSource> bgm = new List<AudioSource>();

    private bool bgmPlaying;
    private int currentTrack;

    public List<AudioSource> sfx = new List<AudioSource>();

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (bgmPlaying == true) {
            if (bgm[currentTrack].isPlaying == false) {
                /* currentTrack++;

                if(currentTrack >= bgm.Count)
                {
                    currentTrack = 0;
                }

                bgm[currentTrack].Play(); */

                StartBGM();
            }
        }
    }

    /// <summary>
    /// Stops all music playing
    /// </summary>
    public void StopMusic() {
        titleMusic.Stop();
        foreach(AudioSource track in bgm) {
            track.Stop();
        }
        bgmPlaying = false;
    }

    /// <summary>
    /// Stops all music and plays the title music
    /// </summary>
    public void StartTitleMusic() {
        StopMusic();
        titleMusic.Play();
    }

    /// <summary>
    /// Stops all music and plays the background music at random
    /// </summary>
    public void StartBGM() {
        StopMusic();
        bgmPlaying = true;
        currentTrack = Random.Range(0, bgm.Count);
        bgm[currentTrack].Play();
    }

    /// <summary>
    /// Plays the given SFX index
    /// </summary>
    /// <param name="sfxToPlay">The index for the SFM</param>
    public void PlaySFX(int sfxToPlay) {
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();
    }
}