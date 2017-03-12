using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFShowerSwitch : MonoBehaviour
{
    private bool switchable;
    private bool isON;
    private bool displayLabel;
    bool lockedInRoutine;
    private IEnumerator coroutine;
    [SerializeField]
    private GameObject trapAssociated;
    [SerializeField]
    private float activateRange;
    [SerializeField]
    private Sprite sprite1;
    [SerializeField]
    private Sprite sprite2;

    [SerializeField]
    private AudioClip SHOWER;
    [SerializeField]
    private AudioClip SHOWER_OFF;


    // Use this for initialization
    void Start()
    {
        switchable = false;
        displayLabel = false;
        isON = true;
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
                coroutine = SwitchShower();
                StartCoroutine(coroutine);
            }
        }
    }

    private IEnumerator SwitchShower()
    {
        if (switchable && GameObject.FindGameObjectWithTag("Player").GetComponent<FFPlayerController>().realWorld)
        {
            if (isON)
            {
                Debug.Log("switch off the shower");
                trapAssociated.GetComponent<FFTrapBehaviour>().Desactivate();
                gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
                isON = false;
				GetComponents<AudioSource>()[1].Stop();
				GetComponents<AudioSource>()[2].Stop();
				GetComponent<AudioSource>().PlayOneShot(SHOWER_OFF);
            } else
            {
                Debug.Log("switch on the shower");
                trapAssociated.GetComponent<FFTrapBehaviour>().Activate();
                gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
                isON = true;
                GetComponent<AudioSource>().PlayOneShot(SHOWER_OFF);
				GetComponents<AudioSource>()[1].Play();
				GetComponents<AudioSource>()[2].Play();
			}
        }
        yield return new WaitForSeconds(1f);
        lockedInRoutine = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            displayLabel = true;
            switchable = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            displayLabel = false;
            switchable = false;
        }
    }

    private void OnGUI()
    {
        if (displayLabel && GameObject.Find("Player").GetComponent<FFPlayerController>().realWorld)
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
            GUI.Label(new Rect(pos.x, pos.y, Screen.width, Screen.height), "Press [CTRL]");
        }
    }
}
