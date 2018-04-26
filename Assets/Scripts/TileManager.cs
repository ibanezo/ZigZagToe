using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public GameObject currentTile;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        SpawnTile();
	}

    public void SpawnTile()
    {
        int randomIndex = Random.Range(0, 2);
        currentTile = (GameObject)Instantiate(tilePrefabs[randomIndex], currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position, Quaternion.identity);
    }
}
