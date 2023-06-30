using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform groundCheck;


    // ----
    public int hunger { get; private set; } = 100;
    float timeToReduceByOne = 5;


    // X-Z movement
    float velocity;
    Vector3 movement;
    [SerializeField] float defaultSpeed = 9;

    // Y movement
    Vector3 gravityMovement;
    [SerializeField] float gravity = 20f;
    public float verticalExplosionForceMultiplier { get; set; } = 0.5f;
    float maxGravity = 25;

    float interactionDistance = 3;

    // Taking Objects
    GasolinePistol gasolinePistol;

    // Some bools
    bool isTouchingCeiling;
    public bool isHolding { get; set; }

    // Axis
    float horizontal, vertical;

    // System values
    float startGravity;

    // Other gameobjects
    HeldObject holdingObject; // the one that is being held right now
    //Shotgun heldWeapon;
    [HideInInspector] public PickableObject heldPickable;

    //Other
    [Header("Other stuff")]
    CharacterController character;
    [SerializeField] Computer computer;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask interactiveObjectsMask;
    [SerializeField] LayerMask almostEverything;

    Animator animator;

    Camera mainCamera;
    public Rigidbody objectHolder;


    private void Start()
    {
        character = GetComponent<CharacterController>();
        mainCamera = Camera.main;

        //animator = GetComponent<Animator>();

        velocity = defaultSpeed;
        startGravity = maxGravity;


        StartCoroutine(ReductionHunger());
    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // this variable represents the speed of the player atm (its cant be == 0, cuz the speed == 0 when the Input.GetAxis("Vertical") == 0)
        // this is the speed only when the wlking button is pressed
        velocity = defaultSpeed;

        if (isTouchingCeiling)
        {
            movement.y = 0;
        }

        //Vector3 mainMovement = (transform.right * horizontal + transform.forward * vertical) * velocity;
        //mainMovement = mainMovement.normalized * velocity;
        //movement = mainMovement + gravityMovement;

        movement = (transform.right * horizontal + transform.forward * vertical) * velocity + gravityMovement; // Fix diagonal movement bug (double velocity bug)
        movement = movement.normalized * velocity;

        Gravity();

        character.Move(movement * Time.deltaTime);

        character.Move(forceVector * Time.deltaTime);
    }

    void Gravity()
    {
        if (GroundCheck() && movement.y < -0.1f)
        {
            movement.y = -0.1f;
            maxGravity = -0.25f;
        }
        if (!GroundCheck())
        {
            maxGravity = startGravity;
        }

        movement.y -= gravity * Time.deltaTime;

        gravityMovement.y -= gravity * 2 * Time.deltaTime;
        gravityMovement.y = Mathf.Clamp(movement.y, -maxGravity, 0);

        forceVector = Vector3.MoveTowards(forceVector, Vector3.zero, gravity * Time.deltaTime);
    }

    // Checking for ground underneath us with spherecasting
    bool GroundCheck()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);
    }

    Vector3 forceVector;
    void AddForce(Vector3 forceVector_1)
    {
        forceVector = forceVector_1;
    }

    // ------------------------------------------------------------------ MOVEMENT SECTION IS OVER -----------------------------------------------------------------------

    private void Update()
    {
        // This line shakes camera by animation; its being played if the velocity of player is greater then 0

        //animator.SetFloat("toWalk", Mathf.Abs(Input.GetAxis("Horizontal")) + Mathf.Abs(Input.GetAxis("Vertical")));

        Movement();
        Interaction();
        ReleaseManager();

        if (Input.GetKeyDown(KeyCode.E) && holdingObject != null)
        {
            Eating();
        }

        hunger = Mathf.Clamp(hunger, 0, 100);
    }

    IEnumerator ReductionHunger()
    {
        yield return new WaitForSeconds(timeToReduceByOne);
        hunger--;

        StartCoroutine(ReductionHunger());
    }

    void Eating()
    {
        hunger += holdingObject.GetComponent<Gib>().nutritionalValue;

        Destroy(holdingObject.gameObject);
        Release(holdingObject);
    }

    void Interaction()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, interactionDistance, almostEverything))
        {
            if (!isHolding)
            {
                if (hit.transform.gameObject.layer == 6 || hit.transform.gameObject.layer == 8 || hit.transform.gameObject.layer == 13) // taking body parts, products and bullet shells 
                {
                    HeldObject heldObject = hit.transform.gameObject.GetComponent<HeldObject>();
                    if (heldObject != null)
                    {
                        Take(heldObject);
                    }
                }
                else if (hit.transform.gameObject.layer == 16) // entering PC
                {
                    EnterPC();
                }
                else if (hit.transform.gameObject.layer == 9) // taking gasoline nozzle
                {
                    gasolinePistol = hit.transform.gameObject.GetComponent<GasolinePistol>();

                    if (gasolinePistol.isInCar)
                    {
                        if (gasolinePistol.car.isFilledUp)
                        {
                            gasolinePistol.StopFillingUp();
                            gasolinePistol.Take();

                            isHolding = true;
                        }
                    }
                    else if (!gasolinePistol.isInCar)
                    {
                        gasolinePistol.Take();

                        isHolding = true;
                    }
                }
            }
        }
    }

    void EnterPC()
    {
        computer.enabled = true;
        StartCoroutine(computer.EnterPC());
    }

    void ReleaseManager()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (holdingObject != null)
            {
                Release(holdingObject);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (holdingObject != null)
            {
                Throw(holdingObject, 35);
            }
        }
    }



    void Take(HeldObject heldObject)
    {
        //startScale = new Vector3(transform.localScale.x * transform.parent.localScale.x,
        //    transform.localScale.y * transform.parent.localScale.y,
        //  

        this.holdingObject = heldObject;

        heldObject.SetPlayerController(this);

        //heldObject.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        //heldObject.gameObject.GetComponent<Collider>().enabled = false;

        Rigidbody productRB = heldObject.gameObject.GetComponent<Rigidbody>();
        productRB.useGravity = false;
        //productRB.freezeRotation = true;

        heldObject.transform.SetParent(null);

        heldObject.isHeld = true;
        isHolding = true;
    }

    void Release(HeldObject heldObject)
    {
        Rigidbody productRb = heldObject.gameObject.GetComponent<Rigidbody>();
        productRb.isKinematic = false;
        productRb.useGravity = true;
        productRb.freezeRotation = false;

        heldObject.transform.SetParent(null);

        heldObject.isHeld = false;        
        isHolding = false;
        this.holdingObject = null;
    }

    void Throw(HeldObject heldObject, float force) // for products and body parts
    {
        Release(heldObject);
        heldObject.GetComponent<Rigidbody>().AddForce(mainCamera.transform.forward * force, ForceMode.Impulse);
    }
}


