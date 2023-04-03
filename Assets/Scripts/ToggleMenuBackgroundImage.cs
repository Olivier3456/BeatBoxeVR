using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMenuBackgroundImage : MonoBehaviour
{

    [SerializeField] private Image backgroundImage;
    
    public void ToggleImage()
    {
        backgroundImage.gameObject.SetActive(!backgroundImage.gameObject.activeInHierarchy);
    }


    public void DisplayImage()
    {
        backgroundImage.gameObject.SetActive(true);
    }
}
