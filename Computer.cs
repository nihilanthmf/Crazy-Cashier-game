using UnityEngine;
using System.Collections;

public class Computer : MonoBehaviour
{
    bool toEnter;
    Camera mainCamera;

    Vector3 cameraPosition = new Vector3(-0.25f, 0.4f, 0.245f); //0,4f
    Vector3 cameraRotation = new Vector3(0, 180, 0);
    float lerpSpeed = 2.75f;

    float timeToTurnOnPc = 2.25f;

    [SerializeField] GameObject pcCanvas;
    [SerializeField] GameObject pcWallpapers;
    [SerializeField] GameObject[] gameobjectsToTurnOffWhenInPCMode;
    
    [SerializeField] CharacterController player;
    CameraController cameraController;

    Transform startCameraParent;
    Vector3 startCameraPosition, startCameraRotation;

    private void Start()
    {
        mainCamera = Camera.main;
        cameraController = mainCamera.GetComponent<CameraController>();

        pcCanvas.SetActive(false);

        startCameraParent = mainCamera.transform.parent;
        startCameraPosition = mainCamera.transform.localPosition;
        startCameraRotation = mainCamera.transform.localEulerAngles;
    }

    public IEnumerator EnterPC()
    {
        player.enabled = false;
        cameraController.enabled = false;

        Cursor.lockState = CursorLockMode.None;

        mainCamera.transform.SetParent(transform);

        toEnter = true;

        yield return new WaitForSeconds(timeToTurnOnPc);

        if (!player.enabled && !cameraController.enabled)
        {
            foreach (GameObject item in gameobjectsToTurnOffWhenInPCMode)
            {
                item.SetActive(false);
            }

            pcCanvas.SetActive(true);
            pcWallpapers.SetActive(true);
        }
        else
        {
            QuitPC();
        }
    }

    void QuitPC()
    {
        mainCamera.transform.SetParent(startCameraParent);

        toEnter = false;

        foreach (GameObject item in gameobjectsToTurnOffWhenInPCMode)
        {
            item.SetActive(true);
        }

        pcCanvas.SetActive(false);
        pcWallpapers.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        player.enabled = true;
        cameraController.enabled = true;
    }

    private void Update()
    {
        if (toEnter)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, cameraPosition, lerpSpeed * Time.deltaTime);
            mainCamera.transform.localEulerAngles = Vector3.Lerp(mainCamera.transform.localEulerAngles, cameraRotation, lerpSpeed * Time.deltaTime);
        }
        else
        {
            mainCamera.transform.localPosition = Vector3.MoveTowards(mainCamera.transform.localPosition, startCameraPosition, lerpSpeed * Time.deltaTime);
            mainCamera.transform.localEulerAngles = Vector3.MoveTowards(mainCamera.transform.localEulerAngles, startCameraRotation, lerpSpeed * Time.deltaTime);

            if (mainCamera.transform.localPosition == startCameraPosition && mainCamera.transform.localEulerAngles == startCameraRotation)
            {
                this.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPC();
        }
    }
}
