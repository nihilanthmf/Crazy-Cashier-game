using UnityEngine;
using System.Collections;
using System;

public class Shotgun : MonoBehaviour
{
    // Taking and releasing the gun
    Vector3 heldPosition = new Vector3(0, -1.2f, 0.845f), heldEulerAngles = new Vector3(0, 90, 0);
    Vector3 startPosition, startEulerAngles;
    Vector3 startScale;
    Transform startParent;
    PickableObject pickableObject;

    Animator animator;
    Camera mainCamera;
    GameObject mesh;

    // Shooting visuals
    [SerializeField] ParticleSystem smokeParticles;
    [SerializeField] GameObject fireEffect;
    [SerializeField] GameObject shell;
    [SerializeField] Transform shellSpawner;
    [SerializeField] GameObject bulletImpact;

    Action shellSpawnerAction;

    // Actual shooting
    int ammo = 200; //2
    float damage = 200;
    [SerializeField] LayerMask targetLayer;
    float pushForce = 7500;

    float timeToShoot;
    const float shootOffset = 0.25f;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        pickableObject = GetComponent<PickableObject>();
        mesh = transform.GetChild(0).gameObject;

        startParent = transform.parent;
        startPosition = transform.localPosition;
        startEulerAngles = transform.localEulerAngles;
        startScale = transform.localScale;

        shellSpawnerAction = DelegateFix;
    }

    IEnumerator ShootingVisual()
    {
        smokeParticles.Play();
        shellSpawnerAction = ShellSpawn;
        fireEffect.SetActive(true);

        yield return new WaitForSeconds(0.025f);
        fireEffect.SetActive(false);

        StartCoroutine(mainCamera.GetComponent<CameraController>().Shaking(0.15f, 0.25f));
        animator.Play("Shot");
    }

    // Spawning bullet shells and giviing them some force
    void ShellSpawn()
    {
        GameObject shellInstance = Instantiate(shell, shellSpawner);
        Rigidbody shellRB = shellInstance.GetComponent<Rigidbody>();
        //Destroy(shellRB, 1);
        shellInstance.SetActive(true);
        shellRB.AddRelativeForce(300, 75, 0);
        shellInstance.transform.SetParent(null);
        shellSpawnerAction = DelegateFix;
    }

    void DelegateFix() { } // This has to be here, pls dont delete it

    void Shooting()
    {
        ammo--;

        RaycastHit hitPoint;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitPoint, 200, targetLayer))
        {
            // Checking the existence of certain components on the object we hit
            Target target = null;
            if (hitPoint.transform.parent != null)
            {
                if (hitPoint.transform.parent.GetComponent<Target>() != null)
                {
                    target = hitPoint.transform.parent.GetComponent<Target>();
                }
            }
            //else if (hitPoint.transform.GetComponent<Target>() != null)
            //{
            //    target = hitPoint.transform.GetComponent<Target>();
            //}

            if (target != null) // If we hit something that is not a wall or whatever
            {
                target.ExecuteMethodOnHit(damage, hitPoint.transform.gameObject, mainCamera.transform.forward);
            }
            else // If we hit the wall or whatever
            {
                GameObject bulletImpactExample = Instantiate(bulletImpact, hitPoint.point, Quaternion.LookRotation(hitPoint.normal));
                bulletImpactExample.transform.SetParent(hitPoint.transform);
                Destroy(bulletImpactExample, 3f);
            }

            if (hitPoint.transform.GetComponent<Rigidbody>()) // To apply a force to a rigidbody connected to the hit object    
            {
                hitPoint.transform.GetComponent<Rigidbody>().AddForceAtPosition(mainCamera.transform.forward * pushForce, hitPoint.point);
            }
        }

        StartCoroutine(ShootingVisual());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && pickableObject.isHeld && ammo > 0 && Time.time >= timeToShoot)
        {
            Shooting();
            timeToShoot = Time.time + shootOffset;
        }
    }

    private void FixedUpdate()
    {
        shellSpawnerAction();
    }
}
