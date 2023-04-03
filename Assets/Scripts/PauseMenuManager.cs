using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] private Image mainMenuBackgroundImage;
    [SerializeField] private Image pauseMenuBackgroundImage;
    [SerializeField] private Image endGameBackgroundImage;
    [SerializeField] private TextMeshProUGUI countDownText;


    public void PauseGameIfNotInMainOrPauseMenus()
    {
        if (!mainMenuBackgroundImage.gameObject.activeInHierarchy &&
            !pauseMenuBackgroundImage.gameObject.activeInHierarchy &&
            !countDownText.gameObject.activeInHierarchy &&
            !endGameBackgroundImage.gameObject.activeInHierarchy)
        {
            GameManager.Instance.PauseGame(true);
            pauseMenuBackgroundImage.gameObject.SetActive(true);
            foreach(var button in pauseMenuBackgroundImage.GetComponentsInChildren<Button>())
            {
                print(button.name);
            }
        }
    }
}