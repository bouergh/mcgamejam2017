using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private GameObject player;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, minX, maxX), transform.position.y, transform.position.z);
        // requires camera to be not attached to player
    }
}
