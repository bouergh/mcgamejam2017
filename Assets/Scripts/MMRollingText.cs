using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMRollingText : MonoBehaviour {

    public float endY;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position.y < endY)
            transform.position += new Vector3(0f, 1f, 0f);
	}
}
