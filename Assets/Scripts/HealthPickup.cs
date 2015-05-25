using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour {

    void Start()
    {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Damage d = other.GetComponent<Damage>();
        d.health += 2;
        Destroy(gameObject);
    }
}
