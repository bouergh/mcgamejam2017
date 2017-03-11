using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMObjectScript : MonoBehaviour {

    bool isPicked;
    bool playerCanPick;
    bool lockedInRoutine;
    bool displayLabel;
    private IEnumerator coroutine;
    Vector3 originPosition;

    // Use this for initialization
    void Start () {
        isPicked = false;
        playerCanPick = false;
        lockedInRoutine = false;
        displayLabel = false;
        originPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockedInRoutine)
        {
            bool pickKey = Input.GetButton("Item");
            if (pickKey && playerCanPick) // if pickUp key is pressed && player is on the object
            {
                lockedInRoutine = true;
                coroutine = Pick();
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator Pick()
    {
        if (isPicked)
        {
            Debug.Log("drop the object");
            transform.SetParent(GameObject.Find("Real World").transform);
            transform.position = new Vector3(transform.position.x, originPosition.y, originPosition.z);
            GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().DropObject();
            isPicked = false;
        }
        // to pick object you must be in real world and not already holding an item
        else if (!isPicked && !GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().itemHeld && GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().realWorld)
        {
            Debug.Log("pick the object");
            Transform playerHands = GameObject.FindGameObjectWithTag("Player").transform.Find("Hands");
            transform.SetParent(playerHands);
            transform.localPosition = Vector3.zero;
            GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().PickObject();
            isPicked = true;
            displayLabel = false; // don't display label when object is picked
        }
        yield return new WaitForSeconds(1f);
        lockedInRoutine = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            displayLabel = true;
            playerCanPick = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!isPicked && other.gameObject.CompareTag("Player"))
        {
            displayLabel = false;
            playerCanPick = false;
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
