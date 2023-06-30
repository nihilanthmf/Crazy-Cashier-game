using UnityEngine;

public class TowTruck : MonoBehaviour
{
    [SerializeField] CustomerManager customerManager;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && customerManager.currentCar.isReadyForTowing)
        {
            CallingTow();
        }
    }

    void CallingTow()
    {
        animator.SetBool("Begin", true);
    }

    void StartCarAnimation()
    {
        customerManager.currentCar.GetComponent<Animator>().Play("GettingTowed");

        StartCoroutine(customerManager.CustomerSpawn());
    }
}
