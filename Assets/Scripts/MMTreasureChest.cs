using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMTreasureChest : MonoBehaviour {

    private bool switchable;
    private bool displayLabel;
    bool lockedInRoutine;
    private IEnumerator coroutine;
    Animator animator;
    GameManager manager;

    // Use this for initialization
    void Start()
    {
        switchable = false;
        displayLabel = false;
        animator = GetComponent<Animator>();
        manager = GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().manager;
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

    // Opens the chest if the player is near the chest && the player is in the virtual world
    private IEnumerator SwitchChest()
    {
        if (switchable && !GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().realWorld)
        {
            manager.masterMixer.SetFloat("virtualRealityVol", -80); // cut virtual world sound
            animator.SetBool("TreasureOpen", true); // activate chest opening animation
            GetComponent<AudioSource>().Play(); // play chest opening sound
            yield return new WaitForSeconds(1f);
            manager.ChangeWorld(); // get back to real world
            manager.masterMixer.SetFloat("realWorldVol", -80); // cut real world sound
            yield return new WaitForSeconds(1f);
            GameObject oven = GameObject.FindGameObjectWithTag("Oven");
            oven.GetComponent<Animator>().SetBool("Open", true); // open oven
            oven.GetComponent<AudioSource>().Play(); // play success sound when opening oven
            yield return new WaitForSeconds(0.5f);
            GameObject cookie = oven.transform.Find("Cookie").gameObject;
            cookie.GetComponent<Renderer>().sortingLayerName = "Always Visible"; // make the cookie appear
            cookie.GetComponent<Renderer>().sortingOrder = 1; // get cookie rendered on top of character
            Transform playerMouth = GameObject.FindGameObjectWithTag("Player").gameObject.transform.Find("Mouth");          
            while (cookie.transform.position != playerMouth.position) // get the cookie to move to player's mouth
            {
                float step = 2.5f * Time.deltaTime;
                cookie.transform.position = Vector3.MoveTowards(cookie.transform.position, playerMouth.position, step);
                yield return new WaitForEndOfFrame();
            }
            cookie.GetComponent<Renderer>().enabled = false; // make the cookie invisible
            cookie.GetComponent<AudioSource>().Play(); // play cookie eating sound
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("Credits"); // change to credits scene

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
