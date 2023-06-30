using UnityEngine;

public class GasolinePistol : MonoBehaviour
{
    LineRenderer lineRenderer;

    public Car car { get; private set; }

    [SerializeField] Transform ropePositionOnPump, ropePositionOnPistol;
    [SerializeField] PlayerController playerController;
    [SerializeField] MeshRenderer isCarFilledUpMaterial;
    [SerializeField] Material carFilledUp, carFillingUp, noCar;

    Vector3 gasolinePistolPosition = new Vector3(0.35f, 0.35f, 1f);
    Vector3 gasolinePistolRotation = new Vector3(180, 0, 90);

    Vector3 startPosition;
    Vector3 startRotation;
    Vector3 startScale;
    Transform startParent;

    public bool isHeld { get; private set;  }
    public bool isInCar { get; private set; }

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        startParent = transform.parent;
        startPosition = transform.localPosition;
        startRotation = transform.localEulerAngles;
        startScale = transform.localScale;
    }

    public void Take()
    {
        transform.SetParent(playerController.transform);
        transform.localPosition = gasolinePistolPosition;
        transform.localEulerAngles = gasolinePistolRotation;

        lineRenderer.positionCount = 2;

        isHeld = true;
    }

    public void Release()
    {
        isHeld = false;

        transform.SetParent(startParent);

        transform.localScale = startScale;

        transform.localPosition = startPosition;
        transform.localEulerAngles = startRotation;

        lineRenderer.positionCount = 0;

        playerController.isHolding = false;
    }

    public void StopFillingUp()
    {
        transform.SetParent(startParent);

        transform.localScale = startScale;

        transform.localPosition = startPosition;
        transform.localEulerAngles = startRotation;

        lineRenderer.positionCount = 0;
        isInCar = false;

        if (car != null)
        {
            car.isBeingFilledUp = true;
        }
    }

    private void Update()
    {
        if (isHeld || isInCar)
        {
            lineRenderer.SetPosition(0, ropePositionOnPistol.position);
            lineRenderer.SetPosition(1, ropePositionOnPump.position);
        }

        // changing the color of the object that shows is the car is filled up, still filling up or there is no car at all at the filling up
        if (isInCar)
        {
            if (car.isBeingFilledUp)
            {
                isCarFilledUpMaterial.material = carFillingUp;
            }
            else
            {
                isCarFilledUpMaterial.material = carFilledUp;
                car.gasolinePistol = this;
            }
        }
        else
        {
            isCarFilledUpMaterial.material = noCar;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 10)
        {
            car = collision.GetComponent<Car>();

            if (!car.isBeingFilledUp)
            {
                Release();

                transform.SetParent(collision.transform);

                transform.localPosition = car.gasolinePistolPosition;
                transform.localEulerAngles = car.gasolinePistolRotation;

                lineRenderer.positionCount = 2;
                isInCar = true;

                car.isBeingFilledUp = true;
            }
        }

        if (collision.gameObject.tag == "Gas Station")
        {
            Release();
        }
    }
}
