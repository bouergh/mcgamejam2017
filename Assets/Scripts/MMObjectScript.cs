using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMObjectScript : MonoBehaviour
{

    bool isPicked;
    bool playerCanPick;
    bool displayLabel;
    private IEnumerator coroutine;
    Vector3 originPosition;
    Animator animator;
    Animator playerInstructionAnim;
    [SerializeField]
    string target; // the tag used to specify a target to drop
    bool onTarget;
    [SerializeField]
    string carryZoneName;

    // Use this for initialization
    void Start()
    {
        isPicked = false;
        playerCanPick = false;
        displayLabel = false;
        originPosition = transform.position;
        animator = GetComponent<Animator>();
        if (animator) // check is there is an animator for this object
        {
            animator.SetBool("isPicked", false);
        }
        playerInstructionAnim = GameObject.Find("Instruction").GetComponent<Animator>();
        onTarget = false;
    }

    // Update is called once per frame
    void Update()
    {
		FFPlayerController controller = GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>();

		if (controller && !controller.playerInRoutine)
        {
            bool pickKey = Input.GetButton("Item");
            if (pickKey && playerCanPick) // if pickUp key is pressed && player is on the object
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().RoutineOn();
                Debug.Log("lock routine");

                if (isPicked)
                {
                    coroutine = Drop();
                    StartCoroutine(coroutine);
                }
                else if (!isPicked && !GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().itemHeld
                    && GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().realWorld)
                {
                    coroutine = Pick();
                    StartCoroutine(coroutine);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().RoutineOff();
                }
            }
        }
    }

    private IEnumerator Pick()
    {
        Debug.Log("pick the object");
        Transform carryZone = GameObject.FindGameObjectWithTag("Player").transform.Find(carryZoneName);
        transform.SetParent(carryZone);
        transform.localPosition = Vector3.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().PickObject();
        isPicked = true;
        if(animator) // check is there is an animator for this object
        {
            animator.SetBool("isPicked", true);
        }
        //displayLabel = false; // don't display label when object is picked
        playerInstructionAnim.SetTrigger("NoItem");
        yield return new WaitForSeconds(1f);
        Debug.Log("unlock routine");
        GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().RoutineOff();
    }

    private IEnumerator Drop()
    {
        if (target == "" || (onTarget)) // if there is no drop target or player is on the target
        {
            Debug.Log("drop the object");
            transform.SetParent(GameObject.Find("Real World").transform);
            transform.position = new Vector3(transform.position.x, originPosition.y, originPosition.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().DropObject();
            isPicked = false;
            if (animator) // check is there is an animator for this object
            {
                animator.SetBool("isPicked", false);
            }
        }
        else // not on the target to drop
        {
            GetComponent<AudioSource>().Play(); // play the UH OH sound
        }
        yield return new WaitForSeconds(1f);
        Debug.Log("unlock routine");
        GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().RoutineOff();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //displayLabel = true;
            playerInstructionAnim.SetTrigger("YesItem");
            playerCanPick = true;
            Debug.Log("pickable");
        }
        else if (target != "" && other.gameObject.CompareTag(target)) // is on target to drop object
        {
            onTarget = true;
            Debug.Log("on target !");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isPicked && other.gameObject.CompareTag("Player"))
        {
            //displayLabel = false;
            playerInstructionAnim.SetTrigger("NoItem");
            playerCanPick = false;
            Debug.Log("not pickable");
        }
        else if (target != "" && other.gameObject.CompareTag(target)) // no more on target to drop object
        {
            onTarget = false;
            Debug.Log("leave target !");
        }
    }

    private void OnGUI()
    {
        if (displayLabel && GameObject.Find("Player").GetComponent<FFPlayerController>().realWorld)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(pos.x, pos.y, Screen.width, Screen.height), "Press [space]");
        }
    }
}
