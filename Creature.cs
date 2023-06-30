using UnityEngine;

public class Creature : MonoBehaviour
{
    // Body parts and their gibs
    [SerializeField] GameObject[] bodyParts;
    Rigidbody[] gibs;
    [SerializeField] GameObject head;
    Collider[] bodyPartColliders;

    // Numbers
    public float health = 100f;
    float gibForcevalue = 750;

    private void Start()
    {
        bodyPartColliders = new Collider[bodyParts.Length];
        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyPartColliders[i] = bodyParts[i].GetComponent<Collider>();
        }

        gibs = new Rigidbody[bodyParts.Length];
        for (int i = 0; i < bodyParts.Length; i++)
        {
            gibs[i] = bodyParts[i].transform.GetChild(0).GetComponent<Rigidbody>();
        }
    }

    public void GetHit(float damage, GameObject bodyPart, Vector3 gibForceDirection)
    {
        health -= damage;

        if (bodyPart.transform.childCount != 0)
        {
            Transform gib = bodyPart.transform.GetChild(0);
            gib.SetParent(null);
            bodyPart.SetActive(false);

            //gidForceVector = new Vector3(Random.Range(-1.1f, 1.1f), 0.4f, Random.Range(-1.1f, 1.1f)) * gibForce;
            Vector3 gibForceVector = gibForceDirection;
            gib.gameObject.SetActive(true);
            gib.GetComponent<Rigidbody>().AddForce(gibForceVector * gibForcevalue);
        }
    }

    void Death()
    {
        for (int i = 0; i < bodyPartColliders.Length; i++)
        {
            bodyPartColliders[i].enabled = false;
        }
        for (int i = 0; i < gibs.Length; i++)
        {
            if (bodyParts[i].activeSelf)
            {
                gibs[i].transform.parent = null;
                gibs[i].gameObject.SetActive(true);

                bodyParts[i].SetActive(false);

                Vector3 gibForceVector = new Vector3(Random.Range(-1.1f, 1.1f), 0.4f, Random.Range(-1.1f, 1.1f));
                gibs[i].gameObject.SetActive(true);
                gibs[i].AddForce(gibForceVector * gibForcevalue);
            }
        }

        Destroy(this);
        //Destroy(GetComponent<Customer>());

        //Destroy(gameObject, 10);
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (health <= 0)
        {
            Death();
        }
    }
}
