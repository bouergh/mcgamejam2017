using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FFPlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    public bool facingRight = true;

    public bool itemHeld = false;
    public bool playerInRoutine = false;
    public bool glassesAtFeet = false;

    public AudioMixer masterMixer;
    Animator animator;

    private GameObject glasses;

    public bool realWorld = true;
    // Use this for initialization
    void Start()
    {
        glasses = GameObject.FindGameObjectWithTag("Glasses");
        animator = GetComponent<Animator>();
        // do something so the player starts in one of the worlds
        realWorld = true;
    }

    // Update is called once per frame
    void Update()
    {
        //picking glasses
        if (Input.GetButtonDown("Glasses") && !itemHeld)
        {
            if (glassesAtFeet)
            {
                PickGlasses();
                glassesAtFeet = false;
            }
            else
            {
                //Debug.Log("dropping glasses");
                DropGlasses();
            }
        }
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
        // check if character animation should be Walk or Idle
        if (move != 0)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        try
        {
            GetComponentInChildren<AFGlassesBehaviour>().facingRight = !GetComponentInChildren<AFGlassesBehaviour>().facingRight;
        }
        catch
        { }
    }

    void PickGlasses()
    {
        if (glasses.GetComponent<AFGlassesBehaviour>().Picked())
        {
            realWorld = false;
            ChangeWorld(realWorld);
        }

    }

    void DropGlasses()
    {

        if (glasses.GetComponent<AFGlassesBehaviour>().Dropped())
        {
            realWorld = true;
            ChangeWorld(realWorld);
        }
    }

    public void PickObject()
    {
        itemHeld = true;
        Debug.Log("player pick");
    }

    public void DropObject()
    {
        itemHeld = false;
        Debug.Log("player drop");
    }

    public void RoutineOn()
    {
        playerInRoutine = true;
        Debug.Log("player enter routine");
    }

    public void RoutineOff()
    {
        playerInRoutine = false;
        Debug.Log("player quit routine");
    }

    public void Fear()
    {
        float move = 2f;
        if (facingRight)
        {
            move *= -1;
        }
        transform.Translate(move, 0, 0);
    }

    private void ChangeWorld(bool world)    //true if in the real world, false if not
    {
        if (world)  //i'll change
        {
            // making real world renderers visible and virtual reality ones invisible
            foreach (SpriteRenderer rend in GameObject.Find("Level").GetComponentsInChildren<SpriteRenderer>())
            {
                if (rend.sortingLayerName == "Real World") rend.enabled = true;
                if (rend.sortingLayerName == "Virtual Reality") rend.enabled = false;
            }
            masterMixer.SetFloat("realWorldVol", 0);
            masterMixer.SetFloat("virtualRealityVol", -80);

            //add something to enable the real world triggers and disable the virtual reality triggers
        }
        else
        {
            // making real world renderers invisible and virtual reality ones visible
            foreach (SpriteRenderer rend in GameObject.Find("Level").GetComponentsInChildren<SpriteRenderer>())
            {
                if (rend.sortingLayerName == "Real World") rend.enabled = false;
                if (rend.sortingLayerName == "Virtual Reality") rend.enabled = true;
            }
            masterMixer.SetFloat("realWorldVol", -80);
            masterMixer.SetFloat("virtualRealityVol", 0);

            //add something to enable the real world triggers and disable the virtual reality triggers
        }




    }
}
