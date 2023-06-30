using UnityEngine;

public class Cooking : MonoBehaviour
{
    Camera mainCamera;
    Animator playerAnimator;
    CameraController cameraController;

    PlayerController player;

    [SerializeField] Transform cameraPositionWhenCooking;
    Vector3 cameraPositionOnPlayer;
    Vector3 currentCameraPosition;

    Vector3 currentVelocityOfCamera;
    float smoothDampTime = 0.25f;

    bool isInCookingSession;

    private void Awake()
    {
        mainCamera = Camera.main;
        player = mainCamera.transform.parent.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        cameraController = mainCamera.GetComponent<CameraController>();
    }

    private void Update()
    {
        if (isInCookingSession)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, currentCameraPosition, ref currentVelocityOfCamera, smoothDampTime);
            mainCamera.transform.eulerAngles = Vector3.MoveTowards(mainCamera.transform.eulerAngles, new Vector3(0, 180, 0), smoothDampTime);
        }

        if ( ( Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C) ) && isInCookingSession) //////////////////////
        {
            EndCookingSession();

            isInCookingSession = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && !isInCookingSession && Vector3.Distance(transform.position, player.transform.position) <= 3)
        {
            StartCookingSession();

            isInCookingSession = true;
        }
    }

    void StartCookingSession()
    {
        player.enabled = false;
        playerAnimator.enabled = false;
        cameraController.enabled = false;

        mainCamera.transform.SetParent(null);

        cameraPositionOnPlayer = mainCamera.transform.position;
        currentCameraPosition = cameraPositionWhenCooking.position;


        Cursor.lockState = CursorLockMode.Confined;
    }

    void EndCookingSession()
    {
        player.enabled = true;
        //playerAnimator.enabled = true;
        cameraController.enabled = true;
        currentCameraPosition = cameraPositionOnPlayer;

        mainCamera.transform.SetParent(player.transform);


        Cursor.lockState = CursorLockMode.Locked;
    }
}
