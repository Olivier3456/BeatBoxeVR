using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup perfectCanvasGroup;

    [Space(10)]
    [SerializeField] private Image endGameBackgroundImage;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private TextMeshProUGUI endGameScoreText;

    [SerializeField] TMP_Dropdown musicChoiceDropdown;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    [SerializeField] private Image scoreBackgroundImage;
    [SerializeField] private Image pauseBackgroundImage;

    private ScoreManager scoreManager;

    private void Start()
    {
        BuildMusicChoiceDropdown();
        scoreManager = GetComponent<ScoreManager>();
    }


    private void BuildMusicChoiceDropdown()
    {
        musicChoiceDropdown.ClearOptions();

        // On crée une liste d'options à partir du tableau de musiques.
        var options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < GameManager.Instance.Musics.Length; i++)
        {
            if (!GameManager.Instance.Musics[i].musicDatas.menuMusic)    // (On n'ajoute la musique au dropdown que si ce n'est pas une musique de menu.)
                options.Add(new TMP_Dropdown.OptionData(GameManager.Instance.Musics[i].musicDatas.audioClip.name));
        }

        // On ajoute les options au Dropdown.
        musicChoiceDropdown.AddOptions(options);
    }


    public void DisplayNewMultiplier(int multiplier)
    {
        comboText.text = "X" + multiplier;
    }


    public void DisplayGameOverUI()
    {
        endGameBackgroundImage.gameObject.SetActive(true);
        endGameText.text = "KO!";
        endGameScoreText.text = "You score: " + scoreManager.Score;
    }


    public void DisplayNewScore(int score)
    {
        scoreText.text = score + " points";
    }


    public void DisplayPerfectCanvasGroup()
    {
        StartCoroutine(PerfectCanvasGroupFadeOut(1.5f));
    }


    public void DisplayPauseUI(bool pause)
    {
        scoreBackgroundImage.gameObject.SetActive(!pause);
        pauseBackgroundImage.gameObject.SetActive(pause);
    }


    private IEnumerator PerfectCanvasGroupFadeOut(float duration)
    {
        float elapsedTime = 0.0f;
        float alpha = 1;

        while (alpha >= 0.0f)
        {
            alpha = 1 - (elapsedTime / duration);
            perfectCanvasGroup.alpha = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}