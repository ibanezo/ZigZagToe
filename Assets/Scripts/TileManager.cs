using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public GameObject[] tilePrefabs;
    public GameObject currentTile;

    private Stack<GameObject> leftTiles = new Stack<GameObject>();
    private Stack<GameObject> topTiles = new Stack<GameObject>();

    private static TileManager tileManager = null;

    public Stack<GameObject> LeftTiles
    {
        get
        {
            return leftTiles;
        }

        set
        {
            leftTiles = value;
        }
    }

    public Stack<GameObject> TopTiles
    {
        get
        {
            return topTiles;
        }

        set
        {
            topTiles = value;
        }
    }

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

    public void CreateTiles(int amount)
    {
        for (int i = 0; i<amount; i++)
        {
            GameObject leftTile = Instantiate(tilePrefabs[0]);
            leftTile.name = "LeftTile";
            leftTile.SetActive(false);
            LeftTiles.Push(leftTile);

            GameObject topTile = Instantiate(tilePrefabs[1]);
            topTile.name = "TopTile";
            topTile.SetActive(false);
            TopTiles.Push(topTile);
        }
    }

    public void SpawnTile()
    {

        if (LeftTiles.Count == 0 || TopTiles.Count == 0)
        {
            CreateTiles(10);
        }

        int randomIndex = Random.Range(0, 2);

        if(randomIndex == 0)
        {
            GameObject tmp = LeftTiles.Pop();
            tmp.SetActive(true);
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = tmp;
        }
        else if (randomIndex == 1)
        {
            GameObject tmp = TopTiles.Pop();
            tmp.SetActive(true);
            tmp.transform.position = currentTile.transform.GetChild(0).transform.GetChild(randomIndex).position;
            currentTile = tmp;
        }

        int spawnPickUp = Random.Range(0, 10);

        if(spawnPickUp == 0)
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }

    }
}
