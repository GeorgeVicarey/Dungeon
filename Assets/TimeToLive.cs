using UnityEngine;
using System.Collections;

public class TimeToLive : MonoBehaviour {

    public float timeToLive;
    public float ttl;

	// Use this for initialization
	void Start () {
        timeToLive = ttl; 
	}
	
	// Update is called once per frame
	void Update () 
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
        
	}

    void OnTriggerEnter2D()
    {
        Destroy(gameObject);
    }
}
