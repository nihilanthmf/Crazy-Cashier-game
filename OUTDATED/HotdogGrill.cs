using UnityEngine;

public class HotdogGrill : MonoBehaviour
{
    [SerializeField] Transform[] cookPositions;

    bool[] freePosition;

    public float timeToCook { get; set; }  = 10;


    private void Start()
    {
        freePosition = new bool[cookPositions.Length];

        for (int i = 0; i < freePosition.Length; i++)
        {
            freePosition[i] = true;
        }
    }

    public Transform FindFreePosition()
    {
        Transform returnPosition = transform;
        for (int i = 0; i < cookPositions.Length; i++)
        {
            if (freePosition[i])
            {
                returnPosition = cookPositions[i];
                freePosition[i] = false;
                break;
            }
        }

        return returnPosition;
    }

    public bool isFree()
    {
        bool returnValue = false;
        for (int i = 0; i < freePosition.Length; i++)
        {
            if (freePosition[i])
            {
                returnValue = true;
                break;
            }
        }

        return returnValue;
    }

    public void MakeSlotFree(Transform slotTransform)
    {
        for (int i = 0; i < cookPositions.Length; i++)
        {
            if (cookPositions[i] == slotTransform)
            {
                freePosition[i] = true;
            }
        }
    }

    public void MakeSlotBusy(Transform slotTransform)
    {
        for (int i = 0; i < cookPositions.Length; i++)
        {
            if (cookPositions[i] == slotTransform)
            {
                freePosition[i] = false;
            }
        }
    }

    //public bool isBusy { get; set; }
}
