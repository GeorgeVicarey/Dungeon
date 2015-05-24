using UnityEngine;
using System.Collections;

public class Zoom : MonoBehaviour {

   
    

	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {
	
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            Camera.main.orthographicSize--;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            Camera.main.orthographicSize++;
        }
        
        
	}
}
