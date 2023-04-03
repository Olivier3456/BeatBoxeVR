using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingElement : MonoBehaviour
{
    public enum ElementType
    {
        LEFTJAB,
        RIGHTJAB,
        LEFTUPPERCUT,
        RIGHTUPPERCUT,
        LEFTHOOK,
        RIGHTHOOK,
        TODODGE,
        TOPARRY
    }
    [NonSerialized]
    public float _speed;

    public ElementType _type;

    [NonSerialized]
    public float HitPrecisionTreshold;
    [NonSerialized]
    public float ParryPrecisionTreshold;
    [NonSerialized]
    public float PerfectTimeTreshold;
    [NonSerialized]
    public float perfectTime;
    [NonSerialized]
    public float maxScore;

    protected AudioSource _punchAudioSource;


    protected GameManager _gameManager;
    protected ScoreManager _scoreManager;
    protected LayerMask _playerMask;
    protected float perfectTimeCounter;

  //  private Renderer[] _renderers;
    private bool gameIsPaused = false;

    private void OnEnable()
    {

        GameManager.Instance.OnPauseStateChange.AddListener(TogglePauseElement);
        gameIsPaused = false;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnPauseStateChange?.RemoveListener(TogglePauseElement);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _gameManager = GameManager.Instance;
        _scoreManager = GameManager.Instance.GetComponent<ScoreManager>();
        _playerMask = _gameManager.PlayerMask;
     //   _renderers = GetComponentsInChildren<Renderer>();

        _punchAudioSource = GameObject.Find("Punch Audio Source").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!gameIsPaused)
        {
            transform.position += (_speed * Time.deltaTime) * Vector3.back;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //print("entered a trigger: " + other);
        if (other.CompareTag("Bounds"))
        {
            gameObject.SetActive(false);
        }
    }

    public void TogglePauseElement(bool isPause)
    {
        if (isPause) transform.position += Vector3.up * 1000;
        else transform.position -= Vector3.up * 1000;


        //foreach (var renderer in _renderers)
        //{
        //    renderer.enabled = !isPause;
        //}

        gameIsPaused = isPause;
    }



}
