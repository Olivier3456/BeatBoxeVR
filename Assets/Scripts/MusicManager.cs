using CCSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicAudioSource;
    public AudioSource MusicAudioSource { get { return musicAudioSource; } }

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        FindAndStartMenuMusic();
    }


    public void Play()
    {
        musicAudioSource.Play();
    }


    public void Pause()
    {
        musicAudioSource.Pause();
    }

    public void Pause(bool pause)
    {
        if (pause) { musicAudioSource.Pause(); }
        else
        {
            if (musicAudioSource.clip !=
                GameManager.Instance.Musics[GameManager.Instance.ActualMusicIndex].musicDatas.audioClip)
            {
                musicAudioSource.clip =
                    GameManager.Instance.Musics[GameManager.Instance.ActualMusicIndex].musicDatas.audioClip;
                musicAudioSource.Play();
            }
            else
            {
                musicAudioSource.UnPause();
            }
        }
    }    
    

    public void UnPause()
    {
        musicAudioSource.UnPause();
    }


    public void Stop()
    {
        musicAudioSource.Stop();
    }


    public void ChangeMusic(Database.MusicDatas musicDatas, bool play)
    {
        musicAudioSource.clip = musicDatas.audioClip;

        if (play) { Play(); }
    }


    public void FindAndStartMenuMusic()
    {
        for (int i = 0; i < GameManager.Instance.Musics.Length; i++)
        {
            if (GameManager.Instance.Musics[i].musicDatas.menuMusic)
            {
                ChangeMusic(GameManager.Instance.Musics[i].musicDatas, true);
                return;
            }
        }
    }



    public void SwitchFromMenuMusicToActualSelectedMusic()
    {
        musicAudioSource.clip =
            GameManager.Instance.Musics[GameManager.Instance.ActualMusicIndex].musicDatas.audioClip;

        musicAudioSource.Play();       
    }
}
