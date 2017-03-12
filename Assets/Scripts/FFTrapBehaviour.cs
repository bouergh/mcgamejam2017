using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFTrapBehaviour : MonoBehaviour {

    public bool danger;
    [SerializeField]
    private Sprite sprite1;
    [SerializeField]
    private Sprite sprite2;

    [SerializeField]
    private AudioClip sonAction;

    //initialisation : au début le danger est actif
    void Start () {
        danger = true;
	}

    private void Update()
    {

    }

    //provoque la peur du personnage
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && danger && !other.gameObject.GetComponent<FFPlayerController>().realWorld)
        {
            other.GetComponent<FFPlayerController>().Fear();
            GetComponent<AudioSource>().PlayOneShot(sonAction);
            Debug.Log("Peur !");
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && danger && !other.gameObject.GetComponent<FFPlayerController>().realWorld)
        {
            other.GetComponent<FFPlayerController>().Fear();
            GetComponent<AudioSource>().PlayOneShot(sonAction);
            Debug.Log("Peur !");
        }
    }
    //Permet de désactiver le piège
    public void Desactivate()
    {
        danger = false;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite2;
    }
    //Permet d'activer le piège
    public void Activate()
    {
        danger = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite1;
    }
}
