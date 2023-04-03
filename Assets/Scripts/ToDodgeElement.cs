using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToDodgeElement : MovingElement
{

    private float _maxBlockingAngle;
    private float _maxBlockingDistance;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _type = ElementType.TODODGE;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if ((_playerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Debug.Log("Hit with Player");
            _scoreManager.UpdateMultiplier(-2);
            gameObject.SetActive(false);
            _punchAudioSource.Play();
        }
    }

}
