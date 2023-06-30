using UnityEngine;
using System.Collections;

public class Car : MonoBehaviour
{
    [SerializeField] TowTruck towTruck;

    public GameObject customer;

    public Vector3 gasolinePistolPosition, gasolinePistolRotation;

    public bool isBeingFilledUp { get; set; }
    public bool isFilledUp { get; private set; }

    public bool isReadyForTowing { get; set; }

    float timeToFullyFillUp = 10; // 20

    Animator animator;

    public GasolinePistol gasolinePistol;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void GettingAway(Customer customer)
    {
        animator.Play("Getting Away");
        customer.gameObject.SetActive(false);

        gasolinePistol.StopFillingUp();
        gasolinePistol.Release();

        customer.hasGoneAway = true;
    }

    void Arrived()
    {
        customer.SetActive(true);
    }

    void MakingCarChild()
    {
        transform.parent = towTruck.transform;

        Destroy(animator);
    }

    private void Update()
    {
        if (isBeingFilledUp)
        {
            StartCoroutine(FillingUp());
        }
    }

    IEnumerator FillingUp()
    {
        yield return new WaitForSeconds(timeToFullyFillUp);
        isFilledUp = true;
        isBeingFilledUp = false;
    }
}
