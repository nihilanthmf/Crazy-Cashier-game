using UnityEngine;

public class Ingredient : MonoBehaviour
{
    Rigidbody rb;

    Vector3 currentVelocity; // for Vector3.SmoothDamp
    float smoothTime = 0.1375f;

    public bool isCooked { get; set; }

    public bool isHeld { get; set; }

    [SerializeField] PlayerController playerController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }

    private void Update()
    {
        if (isHeld)
        {       
            //transform.localPosition = Vector3.Lerp(transform.localPosition, holdingPosition, 1f * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, playerController.objectHolder.transform.position, ref currentVelocity, smoothTime);
        }
    }
}
