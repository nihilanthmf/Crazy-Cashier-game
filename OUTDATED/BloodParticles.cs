using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodParticles : MonoBehaviour
{
    [SerializeField] GameObject bloodPuddle;

    List<ParticleCollisionEvent> collisionEvents;

    GameObject bloodPuddleInstance;
    ParticleSystem particles;

    bool toMakeBlood;

    private void Start()
    {
        particles = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }


    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "For Blood")
        {
            ParticleCollisionEvent collisionEvent = new ParticleCollisionEvent();

            StartCoroutine(BloodAppearing(collisionEvent.intersection));
            print(collisionEvent.intersection);

            int numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);
            Vector3 pos = collisionEvents[0].intersection;
            print(pos);
        }
    }

    IEnumerator BloodAppearing(Vector3 puddlePosition) // yPosition == floor, for the blood not to fly
    {
        yield return new WaitForSeconds(1f);

        toMakeBlood = true;
        bloodPuddleInstance = Instantiate(bloodPuddle);

        bloodPuddleInstance.SetActive(true);

        bloodPuddleInstance.transform.position = puddlePosition;
        bloodPuddleInstance.transform.eulerAngles = new Vector3(90, 0, 0);

        //bloodPuddleInstance.transform.position = collision.contacts[collision.contacts.Length - 1].point + new Vector3(0, 0.05f, 0);
        //bloodPuddleInstance.transform.eulerAngles = collision.contacts[collision.contacts.Length - 1].normal;

        bloodPuddleInstance.transform.localScale = Vector3.zero;
    }
}
