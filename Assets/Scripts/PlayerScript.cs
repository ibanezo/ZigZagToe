using System.Collections;
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
                
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, -Vector3.up);

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
            }

            
        }
    }


}
