using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AFGlassesBehaviour : MonoBehaviour {

    public bool pickable;
    public bool droppable;
    private Transform eyes;
    private Vector2 originPosition;
    public bool facingRight;

	// Use this for initialization
	void Start () {
        pickable = true;
        droppable = false;
        originPosition = transform.position;
        facingRight = true;
        eyes = GameObject.Find("Eyes").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool Picked()
    {
        if (pickable)   //first lines to prevent wrong things from happening
        {
            pickable = false;
            droppable = true;
            //Debug.Log("I got picked ! -Glasses");   //replace by putting glasses as children of the "Eyes" children of the player
            transform.SetParent(eyes);
            transform.localPosition = Vector2.zero;
            if (eyes.parent.GetComponent<FFPlayerController>().facingRight != facingRight)
            {
                Debug.Log("flipping the glasses when picking them !");
                FlipGlasses();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Dropped()
    {
        if (droppable)  //first lines to prevent wrong things from happening
        {
            droppable = false;
            pickable = true;
            Debug.Log("I got dropped ! -Glasses");
            transform.SetParent(null);
            transform.position = new Vector2(transform.position.x, originPosition.y);   
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!droppable && collision.tag == "Player")
        {
            collision.GetComponent<FFPlayerController>().glassesAtFeet = true;
            //Debug.Log("glasses are down here !"); //replace by "showing the button to pick up glasses"
            eyes = collision.transform.FindChild("Eyes");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!droppable && collision.tag == "Player") collision.GetComponent<FFPlayerController>().glassesAtFeet = false;
        //Debug.Log("glasses are not down there anymore !"); //replace by "hiding the button to pick up glasses"
    }

    public void FlipGlasses()
    {
        Debug.Log("I'm flipping ! -Glasses");
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.y *= -1;  // object is turned so that could be a problem, I replaced x by y (copied from Flip on player)
        transform.localScale = scale;
    }
}
