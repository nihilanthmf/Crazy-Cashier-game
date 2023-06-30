using UnityEngine;

public class BloodRecognition : MonoBehaviour
{
    [SerializeField] Creature creature;
    Customer customer;

    [SerializeField] LayerMask forObstaclesCheck;

    private void Start()
    {
        customer = creature.gameObject.GetComponent<Customer>();
    }

    /// <summary>
    /// This one is to check for obstacle that customer should not be able to see through
    /// </summary>
    bool CheckForBloodWithRaycast(Vector3 rayDestination)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rayDestination, out hit, Vector3.Distance(transform.position, rayDestination), forObstaclesCheck))
        {
            if (hit.transform.tag == "Blood" || hit.transform.tag == "Bullet Shell" || hit.transform.tag == "Body Part" || hit.transform.tag == "Shotgun")
            {
                print(hit.transform.name);
                return true;
            }
            else
            {
                return false;
            }
            //if (hit.transform.tag == "Blood" || hit.transform.tag == "Bullet Shell" || hit.transform.tag == "Body Part" || hit.transform.tag == "Shotgun")
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (creature.health > 0)
        {
            if (other.gameObject.tag == "Blood" || other.gameObject.tag == "Bullet Shell" || other.gameObject.tag == "Body Part" || other.gameObject.tag == "Shotgun")
            {
                if (other.gameObject.activeSelf)
                {
                    if (CheckForBloodWithRaycast(other.transform.position))
                    {
                        StartCoroutine(customer.GettingAway());
                    }
                }
            }
        }
    }
}
