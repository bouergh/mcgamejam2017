using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{


    public AudioMixer masterMixer;
    private bool realWorld;

    // Use this for initialization
    void Start()
    {
        realWorld = true; //starting in the real world
    }

    public bool ChangeWorld()
    {
        realWorld = !realWorld;
        if (realWorld)  //i'll change
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
        return realWorld;
    }
}
