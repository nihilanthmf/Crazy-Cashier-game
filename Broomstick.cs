using UnityEngine;

public class Broomstick : MonoBehaviour
{
    float washingSpeed = 0.05f;

    float timeBetweenTaps;
    float timeAtTap;

    private void Update()
    {
        timeBetweenTaps = Time.time - timeAtTap;

        if (Input.GetMouseButtonDown(0))
        {
            timeAtTap = Time.time;
        }
    }

    void Washing(Transform bloodPuddle)
    {
        float deltaWash = washingSpeed / timeBetweenTaps;

        if (timeBetweenTaps <= 0.5f)
        {
            bloodPuddle.localScale = Vector3.MoveTowards(bloodPuddle.localScale, Vector3.zero, deltaWash * Time.deltaTime);
        }
        if (bloodPuddle.localScale.x < 0.1f)
        {
            Destroy(bloodPuddle.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Blood")
        {
            Washing(other.transform);
        }
    }
}
