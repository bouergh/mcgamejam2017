using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFSimpleSwitch : MonoBehaviour
{
    private bool switchable;
    [SerializeField]
    private GameObject trapAssociated;
    [SerializeField]
    private float activateRange;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButton("Item")) StartCoroutine(Switch());
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switchable = true;
            Debug.Log("douche is switchable");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") switchable = false;
    }
    */

    IEnumerator Switch()
    {
        if ((GameObject.Find("Player").transform.position-transform.position).magnitude<activateRange)
        {
            Debug.Log("douche switched off");
            trapAssociated.GetComponent<FFTrapBehaviour>().danger = !trapAssociated.GetComponent<FFTrapBehaviour>().danger;
        }
        yield return null;
    }
}
