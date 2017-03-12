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

    public GameManager manager;
    Animator animator;

    private GameObject glasses;


    [SerializeField]
    private AudioClip sonPeur;
    [SerializeField]
    private AudioClip sonPorter;
    [SerializeField]
    private AudioClip sonPoser;
    [SerializeField]
    private AudioClip sonLunettes;

    public bool realWorld = true;
    // Use this for initialization
    void Start()
    {
        glasses = GameObject.FindGameObjectWithTag("Glasses");
        animator = GetComponent<Animator>();
        // do something so the player starts in one of the worlds
        realWorld = true;

        PickGlasses();
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
            realWorld = manager.ChangeWorld();
        }

    }

    void DropGlasses()
    {

        if (glasses.GetComponent<AFGlassesBehaviour>().Dropped())
        {
            realWorld = manager.ChangeWorld();
        }
    }

    public void PickObject()
    {
        itemHeld = true;
        GetComponent<AudioSource>().PlayOneShot(sonPorter);
        Debug.Log("player pick");
    }

    public void DropObject()
    {
        itemHeld = false;
        GetComponent<AudioSource>().PlayOneShot(sonPoser);
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
        float i,j;
        float move = 2f;
        if (facingRight)
        {
            move *= -1;
        }
        transform.Translate(move, 0, 0);
        GetComponent<AudioSource>().PlayOneShot(sonPeur);
<<<<<<< HEAD
=======
        StartCoroutine(Freeze());
    }

    private IEnumerator Freeze()
    {
        float previousSpeed = maxSpeed;
        maxSpeed = 0f;
        yield return new WaitForSeconds(.5f);
        maxSpeed = previousSpeed;
>>>>>>> a8817856aebf629ed960898612f324cd44940a2e
    }
}
