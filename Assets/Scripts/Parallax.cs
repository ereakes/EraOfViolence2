using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    // private means not editable in unity, the rest of this means that length and start pos are tied together??
    private float length, startpos;

    // This creates a game object section in unity for an object to be placed in
    public GameObject cam;

    // allows you to change the strength of the effect in unity
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // the distance of the parallax effect is the gameobject position times the parallax effect specified in unity, and the the position is moved
    void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
    }
}
