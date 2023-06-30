using System;
using UnityEngine;

public class Target : MonoBehaviour
{
    Action<float, GameObject, Vector3> methodOnHit;

    private void Start()
    {
        if (GetComponent<Creature>())
        {
            methodOnHit = GetComponent<Creature>().GetHit;
        }
        else if (GetComponent<GasStation>())
        {
            methodOnHit = GetComponent<GasStation>().GetHit;
        }
        else if (tag == "2")
        {

        }
    }

    public void ExecuteMethodOnHit(float damage, GameObject bodyPart, Vector3 gibForceDirection)
    {
        methodOnHit(damage, bodyPart, gibForceDirection);
    }
}
