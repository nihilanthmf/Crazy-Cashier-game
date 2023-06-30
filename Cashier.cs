using UnityEngine;

public class Cashier : MonoBehaviour
{
    [HideInInspector] public Customer currentCustomer;
    [HideInInspector] public GameObject heldProduct;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && heldProduct != null)
        {
            print(heldProduct.name);
            currentCustomer.GettingOrder(heldProduct);            
        }
    }
}
