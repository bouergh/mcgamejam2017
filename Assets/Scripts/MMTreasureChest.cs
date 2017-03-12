using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMTreasureChest : MonoBehaviour {

    private bool switchable;
    private bool displayLabel;
    bool lockedInRoutine;
    private IEnumerator coroutine;
    Animator animator;

    // Use this for initialization
    void Start()
    {
        switchable = false;
        displayLabel = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockedInRoutine)
        {
            bool switchKey = Input.GetButton("Item");
            if (switchKey && switchable) // if pickUp key is pressed && player is on the switch
            {
                lockedInRoutine = true;
                coroutine = SwitchChest();
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator SwitchChest()
    {
        if (switchable && GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().realWorld)
        {
            animator.SetBool("TreasureOpen", true);
        }
        yield return new WaitForSeconds(0.25f);
        lockedInRoutine = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player enter chest zone");
            displayLabel = true;
            switchable = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player leave chest zone");
            displayLabel = false;
            switchable = false;
        }
    }

    private void OnGUI()
    {
        if (displayLabel && !GameObject.Find("Player").GetComponent<FFPlayerController>().realWorld) // display chest label when in virtual world
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(pos.x, pos.y, Screen.width, Screen.height), "Press [CTRL]");
        }
    }
}
