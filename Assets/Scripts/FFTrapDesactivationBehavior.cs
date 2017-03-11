using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTrapDesactivationBehavior : MonoBehaviour {

    bool closeEnough;
    [SerializeField]
    GameObject trapAssociated;
    [SerializeField]
    GameObject solutionAssociated;
    [SerializeField]
    private float disablingRange;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        if((transform.position - solutionAssociated.transform.position).magnitude < disablingRange)
        {
            trapAssociated.GetComponent<FFTrapBehaviour>().Desactivate();
        }
        else
        {
            trapAssociated.GetComponent<FFTrapBehaviour>().Activate();
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject==solutionAssociated)
        {
            Debug.Log("objet posey");
            trapAssociated.GetComponent<FFTrapBehaviour>().Desactivate();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject==solutionAssociated)
        {
            Debug.Log("objet parti");
            trapAssociated.GetComponent<FFTrapBehaviour>().Activate();
        }
    }

    */
}
