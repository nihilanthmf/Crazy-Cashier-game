using UnityEngine;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    [SerializeField] Car carExample;

    [SerializeField] Vector3 startCarPosition;

    [SerializeField] Cashier cashier;

    [SerializeField] Dialog dialog;

    float timeBetweenCustomers = 20;

    public Customer currentCustomer { get; private set; }
    public Car currentCar { get; private set; }

    private void Start()
    {
        StartCoroutine(CustomerSpawn(0));
    }

    private void Update()
    {
        if (currentCustomer != null)
        {
            if (!currentCustomer.isDead)
            {
                if (currentCustomer.hasGoneAway)
                {
                    StartCoroutine(CustomerSpawn());
                }
            }
            else
            {
                currentCar.isReadyForTowing = true;

                dialog.DeleteDialog();
            }
        }
    }

    public IEnumerator CustomerSpawn()
    {
        currentCustomer = null;

        yield return new WaitForSeconds(timeBetweenCustomers);

        currentCar = Instantiate(carExample);
        currentCar.gameObject.SetActive(true);
        currentCar.transform.position = startCarPosition;

        currentCustomer = currentCar.customer.GetComponent<Customer>();
        cashier.currentCustomer = currentCustomer;
    }

    /// <summary>
    /// This reload is used for the first time calling out the customer
    /// </summary>
    IEnumerator CustomerSpawn(float time) // for the first one
    {
        currentCustomer = null;

        yield return new WaitForSeconds(time);

        currentCar = Instantiate(carExample);
        currentCar.gameObject.SetActive(true);
        currentCar.transform.position = startCarPosition;

        currentCustomer = currentCar.customer.GetComponent<Customer>();
        cashier.currentCustomer = currentCustomer;
    }
} 
