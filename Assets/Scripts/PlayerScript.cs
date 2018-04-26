﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public float Speed;
    private Vector3 Direction;


    // Use this for initialization
    void Start () {
        Direction = Vector3.zero;

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            if(Direction == Vector3.forward)
            {
                Direction = Vector3.left;
            }
            else
            {
                Direction = Vector3.forward;
            }
        }

        float amountToMove = Speed * Time.deltaTime;
        transform.Translate(Direction * amountToMove);

	}

}
