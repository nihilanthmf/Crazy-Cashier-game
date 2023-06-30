using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField] Cashier cashier;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            cashier.heldProduct = collision.gameObject;
        }
    }
}
