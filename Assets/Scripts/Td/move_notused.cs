﻿using System.Collections; using System.Collections.Generic; using UnityEngine;  public class move_notused : MonoBehaviour {  	// Use this for initialization 	void Start () { 		 	}      int speed =5;   	// Update is called once per frame 	void Update () {         float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;         float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;         transform.Translate(x, 0, z);         print(x);  	} } 