using UnityEngine;

public class HittableElement : MovingElement
{
    HandVelocity _leftHandVelocity, _rightHandVelocity;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _leftHandVelocity = GameManager.Instance.LeftHand.GetComponent<HandVelocity>();
        _rightHandVelocity = GameManager.Instance.RightHand.GetComponent<HandVelocity>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        perfectTimeCounter += Time.deltaTime;

    }

    protected override void OnTriggerEnter(Collider other)
    {
        //print("entered a collision: " + other);
        base.OnTriggerEnter(other);
        if ((_playerMask.value & (1 << other.transform.gameObject.layer)) > 0) // check if collision with player
        {
            if (CheckedAttackDirection(other, ElementTypeToPositionIndex(_type)) && CheckedAttackHand(other)) // attack is correct only if the correct hand was used AND the correct type of attack was performed
            {
                bool isPerfect = false; //is perfect is true when the timing is perfect
                float timePrecisionMultiplier = CheckTiming(ref isPerfect);
                Debug.Log("Good attack type detected");
                _scoreManager.UpdateScore(CalculateScore(other, ElementTypeToPositionIndex(_type), timePrecisionMultiplier), isPerfect);
                _scoreManager.UpdateMultiplier(isPerfect ? 2 : 1);
            }
            else
            {
                Debug.Log("Attack detected, but not the right one");
                _scoreManager.UpdateMultiplier(-1);
            }
            _punchAudioSource.Play();
        }
        else
        {
            Debug.Log("Missed the target");
            _scoreManager.UpdateMultiplier(-1);
        }
        gameObject.SetActive(false);

    }
    //Checks from which side the attack came, thus telling weather it was the correct attack
    private bool CheckedAttackDirection(Collider other, int positionToCheckIndex)
    {
        return (Mathf.Abs(other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex])
                > Mathf.Abs(other.transform.position[(positionToCheckIndex + 1) % 3] - transform.position[(positionToCheckIndex + 1) % 3])
                && Mathf.Abs(other.transform.position[positionToCheckIndex] - transform.position[positionToCheckIndex])
                > Mathf.Abs(other.transform.position[(positionToCheckIndex + 2) % 3] - transform.position[(positionToCheckIndex + 2) % 3]));
    }

    //Checks if the correct hand was used to attack the target
    private bool CheckedAttackHand(Collider other)
    {
        switch (_type)
        {
            case ElementType.LEFTHOOK: return other.gameObject.layer == 8; //LeftHandLayer
            case ElementType.RIGHTHOOK: return other.gameObject.layer == 9; //RightHandLayer
            case ElementType.LEFTJAB: return other.gameObject.layer == 8; //LeftHandLayer
            case ElementType.RIGHTJAB: return other.gameObject.layer == 9; //RightHandLayer
            case ElementType.LEFTUPPERCUT: return other.gameObject.layer == 8; //LeftHandLayer
            case ElementType.RIGHTUPPERCUT: return other.gameObject.layer == 9; //RightHandLayer
            default:
                Debug.LogError("No index associated to type or not a hittable type");
                return false;

        }
    }

    //Associates the axis to check for each type of attack
    private int ElementTypeToPositionIndex(ElementType type)
    {
        switch (type)
        {
            case ElementType.LEFTHOOK: return 0; //x value
            case ElementType.RIGHTHOOK: return 0;
            case ElementType.LEFTJAB: return 2; //z value
            case ElementType.RIGHTJAB: return 2;
            case ElementType.LEFTUPPERCUT: return 1; //y value
            case ElementType.RIGHTUPPERCUT: return 1;
            default:
                Debug.LogError("No index associated to type or not a hittable type");
                return 0;

        }
    }

    private int CalculateScore(Collider other, int positionToCheckIndex, float timePrecisionMultiplier)
    {
        SphereCollider fist, target;
        fist = (SphereCollider)other;
        target = GetComponent<SphereCollider>();
        float controllerSpeed;
        if (other.gameObject.layer == 9)
            controllerSpeed = _rightHandVelocity.GetSpeed();
        else
            controllerSpeed = _leftHandVelocity.GetSpeed();

        print("speed: " + controllerSpeed);
        Vector3 toCheckAgainst = positionToCheckIndex == 0 ? Vector3.right : positionToCheckIndex == 1 ? Vector3.up : Vector3.forward;
        if (Mathf.Abs(Vector3.Dot(fist.transform.position - transform.position, toCheckAgainst)) > fist.radius + target.radius - HitPrecisionTreshold)
        {
            return (int)(maxScore * controllerSpeed * timePrecisionMultiplier);
        }
        else
        {
            // multipling the max score by the distance between colliders on the given axis divided by the maximum distance that they can be in accross the given axis. The distance being always lower, it makes the score lower the less "precise" the given hit is
            return (int)(maxScore * controllerSpeed * timePrecisionMultiplier *
                (Mathf.Abs(Vector3.Dot(fist.transform.position - transform.position, toCheckAgainst)) / (fist.radius + target.radius)));
        }
    }
    private float CheckTiming(ref bool isPerfect)
    {
        if (Mathf.Abs(perfectTimeCounter - perfectTime) < PerfectTimeTreshold)
        {
            isPerfect = true;
            return 1;
        }
        else
        {
            isPerfect = false;
            return 1 / (1 + Mathf.Abs(perfectTimeCounter - perfectTime));
        }
    }
}
