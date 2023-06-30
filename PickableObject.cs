using UnityEngine;
using System;

public class PickableObject : MonoBehaviour
{
    Action methodToExecute;

    [SerializeField] Vector3 heldPosition = new Vector3(0, -1.2f, 0.845f);
    [SerializeField] Vector3 heldEulerAngles = new Vector3(0, 90, 0);

    int startLayer;

    public bool isHeld { get; private set; }

    GameObject mesh;

    private void Start()
    {
        startLayer = gameObject.layer;

        transform.localPosition = heldPosition;
        transform.localEulerAngles = heldEulerAngles;
    }

    public void PerformAction()
    {
        methodToExecute();
    }

    public void Take()
    {
        mesh = transform.GetChild(0).gameObject;
        mesh.layer = 12;
        for (int i = 0; i < mesh.transform.childCount; i++)
        {
            mesh.transform.GetChild(i).gameObject.layer = 12;
        }
        gameObject.layer = 12;

        isHeld = true;
    }

    public void Release()
    {
        //mesh.layer = startLayer;
        //for (int i = 0; i < mesh.transform.childCount; i++)
        //{
        //    mesh.transform.GetChild(i).gameObject.layer = startLayer;
        //}
        //gameObject.layer = startLayer;

        isHeld = false;
    }
}
