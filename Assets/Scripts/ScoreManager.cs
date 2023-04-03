using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private GameManager gameManager;
    public int Score { get { return score; } }

    private int lowMultiplierCount = 0;
    public int LowMultiplierCount { get { return lowMultiplierCount; } } 

    private int multiplier = 2;
    public int Multiplier { get { return multiplier; } }

    public UnityEvent OnPerfectShot;
    public UnityIntEvent OnMultiplierChanged;
    public UnityIntEvent OnScoreUpdated;

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
    }

    public void UpdateScore(int pointsToAdd, bool isPerfect)
    {
        score += pointsToAdd * multiplier;
        OnScoreUpdated.Invoke(score);

        if (isPerfect)
        {
            OnPerfectShot.Invoke();
        }
    }


    public void UpdateMultiplier(int pointsToAdd)
    {
        multiplier = Mathf.Clamp(multiplier + pointsToAdd, 1, 8);

        OnMultiplierChanged.Invoke(multiplier);


        if (multiplier == 1)
        {
            lowMultiplierCount++;
        }
        else
        {
            lowMultiplierCount = 0;
        }

        if (lowMultiplierCount >= 5)
        {
            gameManager.OnLowMultiplier();
        }
    }

    public void InitialiseScore()
    {
        score = 0;
        multiplier = 2;
        lowMultiplierCount = 0;
    }
}
