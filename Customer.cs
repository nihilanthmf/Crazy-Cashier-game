using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] Transform checkout;
    [SerializeField] Cashier cashier;

    Car myCar;

    [SerializeField] Dialog dialog;
    Creature creature;

    public bool isDead { get; private set; }
    bool approachingToCheckout;

    // Order

    bool hasArrivedOnce;
    public bool isGettingAway { get; private set; }
    public bool hasGoneAway { get; set; }


    string[] beginnings = { "Hey dude! Can i get this babe filled up and some",
                            "Wassup! Fill her up and give me these sweet"};

    string[] extraItems = { "cigarettes", "chocolate bar", "gum" };

    string[] ends = { "in addition! Thanks!", 
                    "please.",  
                    "",
                    "Thanks!"};


    string currentMessageBeginning;
    string currentMessageEnd;
    string currentExtraItem;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        creature = GetComponent<Creature>();

        myCar = transform.parent.GetComponent<Car>();

        agent.SetDestination(checkout.position);


        transform.SetParent(null);

        currentMessageBeginning = beginnings[Random.Range(0, beginnings.Length)]; 
        currentExtraItem = extraItems[Random.Range(0, extraItems.Length)];
        currentMessageEnd = ends[Random.Range(0, ends.Length)];
    }

    private void Update()
    {
        isDead = creature.health <= 0 ? true : false;

        if (HasArrived() && !hasArrivedOnce && agent.destination == new Vector3(checkout.position.x, agent.destination.y, checkout.position.z))
        {
            StartCoroutine(OrderingProcess());

        // this line undeneath is pure chaos, i dont understand why doesnt it work, my mind is blown away by all this shit i decided to add this code to CustomerManager and now it works,
        // do not delete it pls
            cashier.currentCustomer = this; 



            hasArrivedOnce = true;
        }
        if (isGettingAway && HasArrived())
        {
            agent.isStopped = true;

            myCar.GettingAway(this);
        }
    }

    IEnumerator OrderingProcess()
    {
        yield return new WaitForSeconds(0.375f); // Not to start ordering immediately

        StartCoroutine(dialog.ShowDialogLine(currentMessageBeginning + " " + currentExtraItem + " " + currentMessageEnd));
    }

    public void GettingOrder(GameObject givenProduct)
    {
        if (!dialog.isSayingDialogLine)
        {
            if (myCar.isFilledUp && givenProduct.tag == currentExtraItem)
            {
                Destroy(givenProduct);

                StartCoroutine(dialog.ShowDialogLine("Ah nice, thanks!"));
                StartCoroutine(GettingAway());
            }
            else
            {
                StartCoroutine(dialog.ShowDialogLine("Why were you even born? I said give me " + currentExtraItem + "! You little dirty lunatic!"));
            }
        }
    }

    
    public IEnumerator GettingAway()
    {
        agent.SetDestination(myCar.transform.position);

        yield return new WaitForSeconds(0.5f);

        isGettingAway = true;
    }


    // This method checks if the customer has arrived or not
    bool HasArrived()
    {
        float distance = agent.remainingDistance;
        return distance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0;
    }
}
