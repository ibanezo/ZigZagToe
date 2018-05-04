﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

    public float Speed;
    private Vector3 Direction;
    public GameObject particalSystem;
    private bool isDead;
    public GameObject resetButton;

    private int score = 0;
    public Text scoreText;
    public Animator gameOverAnimator;

    public Text menuScore;
    public Text menuBestScore;

    private static string BEST_SCORE = "CURRENT BEST SCORE";

    // Use this for initialization
    void Start () {
        Direction = Vector3.zero;
        isDead = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !isDead)
        {
            score++;
            scoreText.text = score.ToString();
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

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            Instantiate(particalSystem, transform.position, Quaternion.identity);

            score += 3;
            scoreText.text = score.ToString();
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        {
                
            if (transform.position.y < 0)
            {
                // Kill player
                isDead = true;
                resetButton.SetActive(true);
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).transform.parent = null;
                }
            }
            if (transform.position.y < 3.4)
            {
                // Kill player
                isDead = true;
                resetButton.SetActive(true);
                if (transform.childCount > 0)
                {
                    transform.GetChild(0).transform.parent = null;
                }

                menuScore.text = scoreText.text;

                int currentBestScore = PlayerPrefs.GetInt(BEST_SCORE, 0);

                currentBestScore = Mathf.Max(currentBestScore, int.Parse(menuScore.text));
                menuBestScore.text = currentBestScore.ToString();

                PlayerPrefs.SetInt(BEST_SCORE, currentBestScore);

                gameOverAnimator.SetTrigger("GameOverTrigger");
            }
        }
    }
}