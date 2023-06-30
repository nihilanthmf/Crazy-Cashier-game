using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] PickableObject[] inventorySlots;
    [SerializeField] PlayerController playerController;
    PickableObject currentHoldingObject;

    private void Update()
    {
        InputCheck();
    }

    void InputCheck()
    {
        bool currentHoldingObjectHasCHanged = false;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentHoldingObject = inventorySlots[0];
            currentHoldingObjectHasCHanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentHoldingObject = inventorySlots[1];
            currentHoldingObjectHasCHanged = true;
        }
        else if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(1))
        {
            currentHoldingObject = null;
            currentHoldingObjectHasCHanged = true;
        }

        if (currentHoldingObjectHasCHanged) // turning off all the other objects except the holding one
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i] != currentHoldingObject)
                {
                    inventorySlots[i].gameObject.SetActive(false);
                    inventorySlots[i].gameObject.GetComponent<PickableObject>().Release();
                }
                else
                {
                    SettingUpCurrentHoldingObject();
                }
            }
            // Checking if a player holds nothing not to let a player grab a product while holding a shotgun or a mop
            bool everythingIsOff = true;
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].gameObject.activeSelf)
                {
                    everythingIsOff = false;
                }
            }
            if (everythingIsOff)
            {
                playerController.isHolding = false;
            }
        }
    }

    void SettingUpCurrentHoldingObject()
    {
        currentHoldingObject.gameObject.SetActive(true);
        currentHoldingObject.Take();

        playerController.heldPickable = currentHoldingObject;
        playerController.isHolding = true;
    }
}
