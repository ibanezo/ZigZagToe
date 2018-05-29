using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    public static int fallDelay = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            TileManager tileManager = TileManager.getInstance();
            tileManager.SpawnTile();
            StartCoroutine(FallDown());
        }
    }

    IEnumerator FallDown()
    {
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(fallDelay);
        TileManager tileManager = TileManager.getInstance();

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        gameObject.SetActive(false);

        switch (gameObject.name)
        {
            case "LeftTile":
                tileManager.LeftTiles.Push(gameObject);
                break;

            case "TopTile":
                tileManager.TopTiles.Push(gameObject);
                break;
        }
    }
}