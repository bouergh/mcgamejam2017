﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FFPlayerController : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    private float hardMaxSpeed;
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

    Animator instructionAnim;

    // Use this for initialization
    void Start()
    {
        glasses = GameObject.FindGameObjectWithTag("Glasses");
        animator = GetComponent<Animator>();
        // do something so the player starts in one of the worlds
        realWorld = true;
        instructionAnim = GameObject.Find("Instruction").GetComponent<Animator>();
        PickGlasses();
        hardMaxSpeed = maxSpeed;
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
        GetComponent<Rigidbody2D>().velocity = new Vector2(move * hardMaxSpeed, GetComponent<Rigidbody2D>().velocity.y);
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
            //instructionAnim.SetBool("GlassesOn", true);
            instructionAnim.SetTrigger("GlassesOn");
            GetComponent<AudioSource>().PlayOneShot(sonLunettes);
        }

    }

    void DropGlasses()
    {

        if (glasses.GetComponent<AFGlassesBehaviour>().Dropped())
        {
            realWorld = manager.ChangeWorld();
            //instructionAnim.SetBool("GlassesOn", false);
            GetComponent<AudioSource>().PlayOneShot(sonLunettes);
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
        animator.SetTrigger("Fear");
        float move = 2f;
        if (facingRight)
        {
            move *= -1;
        }
        transform.Translate(move, 0, 0);
        GetComponent<AudioSource>().PlayOneShot(sonPeur);
        StartCoroutine(Freeze());
    }

    private IEnumerator Freeze()
    {
        hardMaxSpeed = 0f;
        yield return new WaitForSeconds(.5f);
        hardMaxSpeed = maxSpeed;
    }

    public void EndGame()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        animator.SetBool("Move", false);
        Destroy(this);
    }
}
