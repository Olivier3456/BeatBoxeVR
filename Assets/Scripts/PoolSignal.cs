using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolSignal : MonoBehaviour
{

    public Spawner spawner;

    private MovingElement movingElement;

    private void Awake()
    {       
        movingElement = GetComponent<MovingElement>();
    }

    private void OnDisable()
    {
        spawner.AddToPool(movingElement);
    }
}
