using CCSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class UnityIntEvent : UnityEvent<int> { }

[System.Serializable]
public class UnityBoolEvent : UnityEvent<bool> { }

[System.Serializable]
public class UnityMusicDatasEvent : UnityEvent<Database.MusicDatas, bool> { }


// The Music AudioSource must be attached to the Game Manager game object.

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{






    private bool gameOver = false;
    public bool GameOver { get { return gameOver; } }


    private bool gamePaused = true;
    public bool GamePaused { get { return gamePaused; } }


    [SerializeField] AnimationOnBPM[] animationOnBPMObject;

    [SerializeField] private LayerMask playerMask;
    public LayerMask PlayerMask { get { return playerMask; } }

    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;


    public Transform Head { get { return head; } }
    public Transform LeftHand { get { return leftHand; } }
    public Transform RightHand { get { return rightHand; } }

    [SerializeField] private Database[] musics;
    public Database[] Musics { get { return musics; } }

    private int actualMusicIndex = 0;
    public int ActualMusicIndex { get { return actualMusicIndex; } }


    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance != null) return instance;
            else
            {
                instance = new GameManager();
                return instance;
            }
        }
    }

    private int nextSpawnIndex = 0;

    [SerializeField] private Spawner spawner;

    private bool lastObjectSpawned = false;


    private int nextBeat = 0;


    [Space(10)]
    public UnityEvent OnBeat;

    public UnityBoolEvent OnPauseStateChange;
    public UnityEvent OnGameOver;
    public UnityEvent OnRestartGame;
    public UnityMusicDatasEvent OnMusicChanged;

    private MusicManager musicManager;
    private ScoreManager scoreManager;
    private UIManager uIManager;


    private void Awake()
    {
        instance = this;
        musicManager = GetComponent<MusicManager>();
        scoreManager = GetComponent<ScoreManager>();
        uIManager = GetComponent<UIManager>();
        OnBeat = new UnityEvent();
    }



    private void Start()
    {
#if UNITY_EDITOR
        CheckMusicDatabases();
#endif      

        SetAnimatedObjectsBPM();
    }



    private void Update()
    {
        // On se cale sur le rythme du morceau. Chaque unité de musicTime vaut non pas une seconde mais un écart entre deux beats.
        // Et on tient compte du délai initial, pour que les spawns se calent bien sur les beats de la musique :
        float musicTime = (musicManager.MusicAudioSource.time * musics[actualMusicIndex].musicDatas.Bpm / 60)
                          - musics[actualMusicIndex].musicDatas.firstBpmDelay;

        if (musicTime >= nextBeat)
        {
            nextBeat++;
            OnBeat.Invoke();
        }

        if (!lastObjectSpawned && !gamePaused && !gameOver)
        {
            if (musics[actualMusicIndex].ObjectSpawns.Length > 0 &&
                musicTime >= musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].SpawnBeat)
            {
                spawner.SpawnObject(musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].elementType,
                                    musics[actualMusicIndex].ObjectSpawns[nextSpawnIndex].Lane);

                if (nextSpawnIndex < musics[actualMusicIndex].ObjectSpawns.Length - 1)
                {
                    nextSpawnIndex++;
                }
                else if (nextSpawnIndex == musics[actualMusicIndex].ObjectSpawns.Length - 1) lastObjectSpawned = true;
            }
        }
    }

    public void RestartGame()
    {
        OnRestartGame.Invoke();
        scoreManager.InitialiseScore();
        uIManager.DisplayNewScore(0);
        uIManager.DisplayNewMultiplier(2);
        musicManager.FindAndStartMenuMusic();
        spawner.ResetAllElements();
        gamePaused = true;
        gameOver = false;
        nextSpawnIndex = 0;        
        lastObjectSpawned = false;
        SetAnimatedObjectsBPM();
    }









    public void PauseGame(bool pause)
    {
        gamePaused = pause;
        OnPauseStateChange.Invoke(gamePaused);
    }


    public void ChangeMusic(int musicIndex)
    {
        musicIndex = Mathf.Clamp(musicIndex, 0, musics.Length - 1);
        actualMusicIndex = musicIndex;

        OnMusicChanged.Invoke(musics[musicIndex].musicDatas, true);

        SetAnimatedObjectsBPM();
    }


    private void SetAnimatedObjectsBPM()
    {
        if (animationOnBPMObject.Length > 0)
        {
            for (int i = 0; i < animationOnBPMObject.Length; i++)
            {
                animationOnBPMObject[i].SetAnimatorBPM(musics[actualMusicIndex].musicDatas.Bpm,
                                                       musics[actualMusicIndex].musicDatas.firstBpmDelay);
            }
        }
    }


    private void CheckMusicDatabases()
    {
        for (int i = 0; i < musics.Length; i++)
        {
            if (!musics[i].musicDatas.menuMusic)
            {
                if (musics[i].musicDatas.Bpm == 0)
                {
                    Debug.LogError("Le BPM de la musique " + musics[i].musicDatas.audioClip.name + " n'a pas été renseigné !");
                }

                for (int j = 0; j < musics[i].ObjectSpawns.Length; j++)
                {
                    if (j > 0 && musics[i].ObjectSpawns[j].SpawnBeat < musics[i].ObjectSpawns[j - 1].SpawnBeat)
                    {
                        Debug.LogError("L'objet " + j + " de la musique " + musics[i].musicDatas.audioClip.name +
                                       " a un spawn beat inférieur à l'objet qui le précède dans leur base de données." +
                                       "Les deux objets doivent être intervertis.");
                    }

                    int originalLane = musics[i].ObjectSpawns[j].Lane;
                    musics[i].ObjectSpawns[j].Lane = Mathf.Clamp(musics[i].ObjectSpawns[j].Lane, 1, 3);

                    if (originalLane != musics[i].ObjectSpawns[j].Lane)
                    {
                        Debug.Log("L'objet numero " + j + " de la musique " + musics[i].musicDatas.audioClip.name +
                                  " n'a pas de piste assignée. Il a été placé par défaut sur la piste 1.");
                    }
                }
            }
        }
    }
    public void OnLowMultiplier()
    {
        gameOver = true;
        OnGameOver.Invoke();
        PauseGame(true);
    }

    public void QuitGame()
    {
        // save any game data here
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
             Application.Quit();
#endif
    }
}
