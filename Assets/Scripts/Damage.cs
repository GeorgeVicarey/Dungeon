using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

    public int health = 1;
    public float invuln = 0f;
    int correctlayer;
    public MapGen map;

    void Start()
    {
        correctlayer = gameObject.layer;
        map = MapGen.FindObjectOfType<MapGen>();
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("Trigger!");

        health--;
        invuln = 2f;
        gameObject.layer = 10;
    }

    void Update()
    {

        invuln -= Time.deltaTime;

        if (health == 0)
        {
            die();
        }
        if (invuln <= 0)
        {
            gameObject.layer = correctlayer;
            invuln = 0;
        }
    }

    void die()
    {
        Destroy(gameObject);
        if (this.gameObject.tag == "Enemy");
        {
            map.noOfEnemies--;
        }
    }
}
