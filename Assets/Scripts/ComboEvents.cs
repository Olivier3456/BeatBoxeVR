using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboEvents : MonoBehaviour
{
    
    int comboLevel {get; set;}
    int comboLevelTemp; 

   [SerializeField] ScoreManager GM;
    public UnityEvent<int> comboChangeEvent;

    void Awake()
    { 
        comboLevel = GM.Multiplier;
        comboLevelTemp = comboLevel;     
    }
    

    // Update is called once per frame
    void Update()
    {         
        comboLevel = GM.Multiplier;  
        comboChange(); 
    }


    void comboChange()
    {
        if (comboLevel != comboLevelTemp)
        {
            Debug.Log("comboEvent"+comboLevel);
            comboChangeEvent.Invoke(comboLevel);
            comboLevelTemp = comboLevel;
        }
    }


}

