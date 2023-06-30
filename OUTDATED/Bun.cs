using System.Collections;
using UnityEngine;

public class Bun : MonoBehaviour
{
    Ingredient ingredient;
    [SerializeField] Toaster toaster;

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

        transform.parent = toaster.transform;
        transform.localEulerAngles = new Vector3(0, 0, 90);

        transform.parent = toaster.FindFreePosition();
        currentCookingPlace = transform.parent;
        toaster.MakeSlotBusy(currentCookingPlace);
        transform.localPosition = Vector3.zero;

        gameObject.layer = 0;


        yield return new WaitForSeconds(toaster.timeToCook);


        gameObject.layer = 8;
        GetComponent<Renderer>().material.color = Color.black;

        ingredient.isCooked = true;
    }

    private void Update()
    {
        if (ingredient.isCooked && ingredient.isHeld)
        {
            toaster.MakeSlotFree(currentCookingPlace);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Toaster")
        {
            if (!ingredient.isHeld && oneTimeCook && toaster.isFree())
            {
                StartCoroutine(Cook());
                oneTimeCook = false;
            }
        }
    }
}
