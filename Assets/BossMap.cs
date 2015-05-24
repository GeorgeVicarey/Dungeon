using UnityEngine;
using System.Collections;

public class BossMap : MonoBehaviour {

    public int width = 50, height = 50;
    public GameObject dirt;
    public GameObject Boss;
    public GameObject Player;
    public GameObject eTank;

	// Use this for initialization
	void Start () {
        for (int x = 0; x < width; x++ )
        {
            for (int y = 0; y < height; y++ )
            {
                Instantiate(dirt, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
        Instantiate(Boss, new Vector3(Random.Range(10, width - 10), Random.Range(10, height - 10), -1), Quaternion.identity);
        Instantiate(Player , new Vector3(Random.Range(10, width - 10), Random.Range(10, height - 10), -1), Quaternion.identity);
        Instantiate(eTank, new Vector3(Random.Range(10, width - 10), Random.Range(10, height - 10), -1), Quaternion.identity);
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
