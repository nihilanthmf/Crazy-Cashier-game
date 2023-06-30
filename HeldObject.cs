using UnityEngine;

public class HeldObject : MonoBehaviour
{
    public bool isHeld { get; set; }


    Rigidbody rb;

    PlayerController playerController;

    Vector3 currentVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }


    private void FixedUpdate()
    {
        if (isHeld)
        {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, holdingPosition, 1f * Time.deltaTime);

            Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, playerController.objectHolder.transform.position, ref currentVelocity, 0.03f);

            //Vector3 smoothPosition = Vector3.Lerp(transform.localPosition, playerController.objectHolder.transform.position, 1);
            rb.velocity = (smoothPosition - transform.localPosition) * 50;
        }
    }
}
