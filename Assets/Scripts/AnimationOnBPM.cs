using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationOnBPM : MonoBehaviour
{
    [SerializeField] private Animator animator;

       
    private float musicDelay = 0;

    private float BPM = 0;

    private bool musicBegun = false;

    private void OnEnable()
    {
        GameManager.Instance.OnBeat.AddListener(ResetAnim);
    }
    private void OnDisable()
    {
        if (GameManager.Instance.OnBeat != null)
        GameManager.Instance.OnBeat.RemoveListener(ResetAnim);
    }

    private void ResetAnim()   // L'animation redémarre à chaque beat pour s'assurer qu'elle reste synchro avec la musique.
    {
        animator.SetTrigger("ResetAnim");
    }

    void Update()
    {
        if (GameManager.Instance.GamePaused)
        {
            animator.speed = 0;
        }
        else
        {
            if (musicBegun) // Si la musique a déjà commencé (càd si on sort du menu pause, et non pas du main menu), pas besoin d'appliquer à nouveau le time offset.
            {
                animator.speed = BPM;
            }
            else
            {
                StartCoroutine(ApplyMusicDelay());
            }
        }
    }


    IEnumerator ApplyMusicDelay()
    {
        yield return new WaitForSeconds(musicDelay);
        animator.speed = BPM;
        musicBegun = true;
    }



    public void SetAnimatorBPM(float bpm, float delay)
    {
        BPM = bpm / 60;
        animator.speed = BPM;        
        musicDelay = delay;
        musicBegun = false;
    }
}
