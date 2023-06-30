using System.Collections;
using UnityEngine;

public class Sausage : MonoBehaviour
{
    Ingredient ingredient;
    [SerializeField] HotdogGrill hotdogGrill;

    Rigidbody rb;

    Transform currentCookingPlace;

    bool oneTimeCook = true;

    private void Start()
    {
        ingredient = GetComponent<Ingredient>();
        rb = GetComponent<Rigidbody>();
    }

    IEnumerator Cook()
    {
        rb.isKinematic = true;    

        transform.parent = hotdogGrill.transform;
        transform.localEulerAngles = new Vector3(0, 0, 90);

        transform.parent = hotdogGrill.FindFreePosition();
        currentCookingPlace = transform.parent;
        hotdogGrill.MakeSlotBusy(currentCookingPlace);
        transform.localPosition = Vector3.zero;

        gameObject.layer = 0;


        yield return new WaitForSeconds(hotdogGrill.timeToCook);


        gameObject.layer = 8;
        GetComponent<Renderer>().material.color = Color.black;

        ingredient.isCooked = true;
    }

    private void Update()
    {
        if (ingredient.isCooked && ingredient.isHeld)
        {
            hotdogGrill.MakeSlotFree(currentCookingPlace);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Hotdog Grill")
        {
            if (!ingredient.isHeld && oneTimeCook && hotdogGrill.isFree())
            {
                StartCoroutine(Cook());
                oneTimeCook = false;
            }
        }
    }
}
