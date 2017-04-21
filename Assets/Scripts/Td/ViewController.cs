using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float speed = 50;
    public float mousespeed = 750;

	// Update is called once per frame
	void Update () {
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;
        float y = Input.GetAxis("Mouse ScrollWheel") * mousespeed;
        transform.Translate(new Vector3(h, y, v) * Time.deltaTime ,Space.World);
        
	}
}
