using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountDownBeforeMusic : MonoBehaviour
{
    [SerializeField] private float delay = 1;
    [SerializeField] private int startNumber = 3;

    [SerializeField] private GameObject menuBackgroundImage;
    [SerializeField] private TextMeshProUGUI countDownText;   

    private int repeatCount = 0;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip countAudioClip;

    private MusicManager musicManager;
   

    public UnityEvent OnResumeMusic;

    private void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
    }


    public void LetsGo()
    {
        musicManager.Pause();
        //GameObject.Find("Game Manager").GetComponent<MusicManager>().MusicAudioSource.Pause();        
        repeatCount = 0;
        countDownText.gameObject.SetActive(true);
        StartCoroutine(Repeating());
        menuBackgroundImage.gameObject.SetActive(false);
    }

    private IEnumerator Repeating()
    {
        countDownText.text = (startNumber - repeatCount).ToString();
        if (audioSource != null && countAudioClip != null) { audioSource.PlayOneShot(countAudioClip); }
        yield return new WaitForSeconds(delay);
        repeatCount++;
        if (repeatCount < startNumber)
        {
            StartCoroutine(Repeating());
        }
        else
        {
            countDownText.text = "";
            countDownText.gameObject.SetActive(false);
            
            OnResumeMusic.Invoke();
            GameManager.Instance.PauseGame(false);
        }
    }
}
