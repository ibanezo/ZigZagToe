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
    private SpeedState PlayerSpeedState;

    private bool GameStarted;
    private bool GameRestarted;

    private int velocityTime = 3;
    private int initialSpeed = 10;
    private float checkIfDeadTime = 2f;

    private Rigidbody rigidbody;

    public Animator startGameAnimator;

    public GameObject startGameMenu;
    public GameObject helpGameMenu;
    public GameObject infoGameMenu;

    private enum SpeedState
    {
        NORMAL, FAST, SLOW
    }
    public AudioSource jumpSound;
    public AudioSource SpeedUpSound;
    public AudioSource CollectSound;
    public AudioSource EndSound;
    public AudioSource SlowDownSound;

    private enum UserInput
    {
        NONE, TAP, SWIPE
    }

    private UserInput userInput;
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private bool jumped;

    // Use this for initialization
    void Start () {
        Direction = Vector3.zero;
        isDead = false;
        PlayerSpeedState = SpeedState.NORMAL;
        Speed = initialSpeed;
        rigidbody = GetComponent<Rigidbody>();
        userInput = UserInput.NONE;
        dragDistance = Screen.height * 10 / 100; //dragDistance is 10% height of the screen
        jumped = false;

        string gameRestarted = PlayerPrefs.GetString(TileManager.GAME_RESTARTED_KEY, TileManager.GAME_NOT_RESTARTED);

        if (gameRestarted.Equals(TileManager.GAME_RESTARTED))
        {
            startGameMenu.SetActive(false);
            Direction = Vector3.forward;
            GameStarted = true;
            
        }
        else
        {
            GameStarted = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

        UpdateUserInput();

        if (userInput == UserInput.TAP && !isDead && GameStarted)
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

        initialSpeed = 10 + (score / 40);
        float amountToMove = Speed * Time.deltaTime;
        transform.Translate(Direction * amountToMove);

        if (userInput == UserInput.SWIPE && !isDead && GameStarted && transform.position.y == 3.5 && !jumped)
        {
            jumpSound.Play();
            rigidbody.AddForce(new Vector3(0, 31, 0), ForceMode.Impulse);
            StartCoroutine(ReallowJumping());
            StartCoroutine(CheckIfDead());
        }
        userInput = UserInput.NONE;
    }

    private void UpdateUserInput()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position; //last touch position

                //Check if drag distance is greater than 10% of the screen height
                if (lp.y > fp.y + dragDistance)
                {
                    userInput = UserInput.SWIPE;
                }
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position; //last touch position

                //Check if drag distance is greater than 10% of the screen height
                if (lp.y > fp.y + dragDistance)
                {
                    userInput = UserInput.SWIPE;
                }
                else
                {   //It's a tap as the drag distance is less than 10% of the screen height
                    userInput = UserInput.TAP;
                }
            }
        }
        else
        {
            userInput = UserInput.NONE;
        }
    }

    IEnumerator CheckIfDead()
    {
        yield return new WaitForSeconds(checkIfDeadTime);
        if (transform.position.y < 3.4)
        {
            // Kill player
            KillPlayer();
        }
    }

    IEnumerator ReallowJumping()
    {
        jumped = true;
        yield return new WaitForSeconds(0.2f);
        jumped = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            Instantiate(particalSystem, transform.position, Quaternion.identity);
            CollectSound.Play();
            score += 3;
            scoreText.text = score.ToString();

            initialSpeed = 10 + (score / 40);
        }
        else if (other.tag == "PickUpSlowSpeed")
        {
            other.gameObject.SetActive(false);
            Instantiate(particalSystem, transform.position, Quaternion.identity);
            SlowDownSound.Play();
            StartCoroutine(SlowDown());
        }
        else if (other.tag == "PickUpFastSpeed")
        {
            other.gameObject.SetActive(false);
            Instantiate(particalSystem, transform.position, Quaternion.identity);
            SpeedUpSound.Play();
            StartCoroutine(SpeedUp());
        }
    }

    IEnumerator SlowDown()
    {
        SlowGame();
        yield return new WaitForSeconds(velocityTime);
        if (PlayerSpeedState == SpeedState.SLOW)
        {
            NormalizeGame();
        }
    }

    IEnumerator SpeedUp()
    {
        FastGame();
        yield return new WaitForSeconds(velocityTime);
        if (PlayerSpeedState == SpeedState.FAST)
        {
            NormalizeGame();
        }
    }

    private void SlowGame()
    {
        Speed = (int) (initialSpeed * 0.8);
        PlayerSpeedState = SpeedState.SLOW;
    }

    private void NormalizeGame()
    {
        Speed = initialSpeed;
        PlayerSpeedState = SpeedState.NORMAL;
    }

    private void FastGame()
    {
        Speed = (int) (initialSpeed * 1.2);
        PlayerSpeedState = SpeedState.FAST;
    }


    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        { 
            if (transform.position.y < 3.4)
            {
                KillPlayer();
            }
        }
    }

    private void KillPlayer()
    {
        if (!isDead)
        {
            EndSound.Play();
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

    public void startGame()
    {
        PlayerPrefs.SetString(TileManager.GAME_RESTARTED_KEY, TileManager.GAME_NOT_RESTARTED);
        GameStarted = true;
        Direction = Vector3.forward;
        startGameAnimator.SetTrigger("StartGameTrigger");
    }

    public void exitGame()
    {
        PlayerPrefs.SetString(TileManager.GAME_RESTARTED_KEY, TileManager.GAME_NOT_RESTARTED);
        Application.Quit();
    }

    public void getMenu()
    {
        PlayerPrefs.SetString(TileManager.GAME_RESTARTED_KEY, TileManager.GAME_NOT_RESTARTED);
        Application.LoadLevel(Application.loadedLevel);
    }

    public void returnToMenu()
    {
        infoGameMenu.SetActive(false);
        helpGameMenu.SetActive(false);
        startGameMenu.SetActive(true);
    }

    public void getHelp()
    {
        startGameMenu.SetActive(false);
        helpGameMenu.SetActive(true);
    }

    public void getInfo()
    {
        startGameMenu.SetActive(false);
        infoGameMenu.SetActive(true);
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.SetString(TileManager.GAME_RESTARTED_KEY, TileManager.GAME_NOT_RESTARTED);
    }
}