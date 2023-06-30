using UnityEngine;
using System.Collections;

public class Gib : MonoBehaviour
{
    public int nutritionalValue;

    [SerializeField] GameObject bloodPuddleOriginal;
    ParticleSystem hemorrage;
    bool toMakeBlood;
    GameObject bloodPuddleInstance;
    Rigidbody rb;

    [SerializeField] LayerMask bloodSurface, blood;

    float bloodAppearingSpeed = 0.35f;

    private void Start()
    {
        hemorrage = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();
    }


    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "For Blood" && hemorrage.isPlaying)
    //    {
    //        StartCoroutine(BloodAppearing(collision.contacts[0].point.y));
    //    }
    //}

    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.tag == "For Blood")
    //    {
    //        if (bloodPuddleInstance != null)
    //        {
    //            if (toMakeBlood)
    //            {
    //                bloodPuddleInstance.transform.localScale = Vector3.MoveTowards(bloodPuddleInstance.transform.localScale,
    //                            bloodPuddleExample.transform.localScale, bloodAppearingSpeed * Time.deltaTime);
    //            }
    //        }
    //        else
    //        {
    //            if (hemorrage.isPlaying)
    //            {
    //                StartCoroutine(BloodAppearing(collision.contacts[0].point.y));
    //            }
    //        }
    //    }
    //}


    void BloodAppearing(float yPosition) // yPosition == floor, for the blood not to fly
    {
        bloodPuddleInstance = Instantiate(bloodPuddleOriginal);

        //yield return new WaitForSeconds(0.35f);

        toMakeBlood = true;

        bloodPuddleInstance.SetActive(true);

        bloodPuddleInstance.transform.position = hemorrage.transform.position + new Vector3(0, 0.05f, 0);
        bloodPuddleInstance.transform.position = new Vector3(bloodPuddleInstance.transform.position.x, yPosition + 0.005f, bloodPuddleInstance.transform.position.z);
        bloodPuddleInstance.transform.eulerAngles = new Vector3(90, 0, 0);

        //bloodPuddleInstance.transform.position = collision.contacts[collision.contacts.Length - 1].point + new Vector3(0, 0.05f, 0);
        //bloodPuddleInstance.transform.eulerAngles = collision.contacts[collision.contacts.Length - 1].normal;

        bloodPuddleInstance.transform.localScale = Vector3.one * 0.1f;
    }

    float timeToSpawnNewBloodStain;
    private void Update()
    {
        rb.WakeUp();

        if (!hemorrage.isPlaying)
        {
            toMakeBlood = false;
        }

        //if (hemorrage.isPlaying && bloodPuddleInstance != null)
        //{
        //    bloodPuddleInstance.transform.localScale = Vector3.MoveTowards(bloodPuddleInstance.transform.localScale,
        //            bloodPuddleOriginal.transform.localScale, bloodAppearingSpeed * Time.deltaTime);
        //}


        RaycastHit hit;
        if (Physics.Raycast(hemorrage.transform.position, Vector3.down, 100, blood) && hemorrage.isPlaying && bloodPuddleInstance != null)
        {
            bloodPuddleInstance.transform.localScale = Vector3.MoveTowards(bloodPuddleInstance.transform.localScale,
                    bloodPuddleOriginal.transform.localScale, bloodAppearingSpeed * Time.deltaTime);
        }
        // Checking for surface beneath the hemmorage to spawn blood stain on
        else if (Time.time >= timeToSpawnNewBloodStain && Physics.Raycast(hemorrage.transform.position, Vector3.down, out hit, 100, bloodSurface))
        {
            if (hit.transform.tag == "For Blood" && hemorrage.isPlaying)
            {
                BloodAppearing(hit.point.y);

                timeToSpawnNewBloodStain = Time.time + 0.05f;
            }
        }
    }
}
