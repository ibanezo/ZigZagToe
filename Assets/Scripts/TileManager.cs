using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public GameObject currentTile;

    private static TileManager tileManager = null;

	// Use this for initialization
	void Start () {
        for (int i = 0; i<20; i++)
        {
            SpawnTile();
        }
    }

    public static TileManager getInstance()
    {
        if (tileManager == null)
        {
            tileManager = GameObject.FindObjectOfType<TileManager>();
        }

        return tileManager;
    }
	
	// Update is called once per frame
	void Update () {

	}

    public void SpawnTile()
    {
        int randomIndex = Random.Range(0, 2);
        currentTile = (GameObject)Instantiate(tilePrefabs[randomIndex], currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position, Quaternion.identity);
    }
}
