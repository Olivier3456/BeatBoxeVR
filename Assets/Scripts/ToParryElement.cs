using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToParryElement : MovingElement
{

    private float _maxBlockingAngle;
    private float _maxBlockingDistance;



    private Transform _head, _leftHand, _rightHand;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _head = _gameManager.Head;
        _leftHand = _gameManager.LeftHand;
        _rightHand = _gameManager.RightHand;
        _type = ElementType.TOPARRY;
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
            if (CheckIfBlocking(other))
            {
                Debug.Log("Hit with Layermask and blocking");
                _scoreManager.UpdateScore(CalculateScore(other), false); // checking on the x axis and a parry cannot be timed
            }
            else
            {
                Debug.Log("Player not blocking on contact");
                _scoreManager.UpdateMultiplier(-1);
            }
            _punchAudioSource.Play();
        }
        gameObject.SetActive(false);

    }
    //Checks from which side the attack came, thus telling weather it was the correct attack
    private bool CheckIfBlocking(Collider other)
    {
        if (other)// is a hand
        {
            if (Mathf.Abs(Vector3.Angle(Vector3.forward, other.transform.position - _head.position)) < _maxBlockingAngle)
            {
                if(Mathf.Abs(Vector3.Angle(Vector3.right, _rightHand.position - _leftHand.position)) < _maxBlockingAngle && Vector3.Distance(_rightHand.position, _leftHand.position) < _maxBlockingDistance)
                {
                    return true;
                }
            }

        }
        return false;

    }
    private int CalculateScore(Collider other)
    {
        SphereCollider fist, target;
        fist = (SphereCollider)other;
        target = GetComponent<SphereCollider>();

        if (Mathf.Abs(Vector3.Dot(fist.transform.position - transform.position, Vector3.forward)) > fist.radius + target.radius - HitPrecisionTreshold)
        {
            return (int)maxScore;
        }
        else
        {
            // multipling the max score by the distance between colliders on the given axis divided by the maximum distance that they can be in accross the given axis. The distance being always lower, it makes the score lower the less "precise" the given hit is
            return (int)(maxScore *
                (Mathf.Abs(Vector3.Dot(fist.transform.position - transform.position, Vector3.forward)) / (fist.radius + target.radius)));
        }
    }
}
