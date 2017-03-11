using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FFPlayerController : MonoBehaviour {
    [SerializeField]
    private float maxSpeed;
    public bool facingRight = true;

    public bool itemHeld = false;
    public bool glassesAtFeet = false;

    [SerializeField]
    private GameObject manager;

    Animator animator;

    private GameObject glasses;

    public bool realWorld = true;
    // Use this for initialization
    void Start () {
        glasses = GameObject.FindGameObjectWithTag("Glasses");
        animator = GetComponent<Animator>();
        // do something so the player starts in one of the worlds
        realWorld = true;

        StartCoroutine(Begin());
        
    }

    // Update is called once per frame
    void Update () {
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
        if(move > 0 && !facingRight)
        {
            Flip();
        } else if(move < 0 && facingRight)
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
            realWorld = manager.GetComponent<GameManager>().ChangeWorld();
            
        }

    }

    void DropGlasses()
    {

        if (glasses.GetComponent<AFGlassesBehaviour>().Dropped())
        {
            realWorld = manager.GetComponent<GameManager>().ChangeWorld();
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

    public void Fear()
    {
        float move = 2f;
        if (facingRight)
        {
            move *= -1;
        }
        transform.Translate(move, 0, 0);
    }

    IEnumerator Begin()
    {
        PickGlasses();
        yield return null;
    }
}
