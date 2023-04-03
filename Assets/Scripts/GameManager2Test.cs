using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2Test : MonoBehaviour
{

    int multiplier = 0;

    public int Multiplier { get { return multiplier; } }

    void Start()
    {
        StartCoroutine(changeMultiplicator()) ;
    }

    IEnumerator changeMultiplicator(){
        
        multiplier ++;
        Debug.Log("multiplicator"+multiplier);
        yield return new WaitForSeconds(1f);
        
        StartCoroutine(changeMultiplicator());
        
    }

}
