using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour
{

    //public CNAbstractController RotateJoystick;
    public float speed = 10f;
    public float rotspeed = 180f;
    Damage mo;
    public MapGen map;

    private Transform _transformCache;

    // Use this for initialization
    void Start()
    {
        mo = GetComponent<Damage>();
        map = MapGen.FindObjectOfType<MapGen>();
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20),"Health: " + mo.health.ToString());
        if (map == null)
        {
            return;
        }
        GUI.Label(new Rect(10, 50, 100, 20), "Enemies Left: " + map.noOfEnemies);
    }


    // Update is called once per frame
    void Update()
    {
        
        Quaternion rot = transform.rotation;
        float z = rot.eulerAngles.z;

        z -= Input.GetAxis("Horizontal") * rotspeed * Time.deltaTime;
        rot = Quaternion.Euler(0, 0, z);
        transform.rotation = rot;

        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0);
        pos += rot * velocity;
        transform.position = pos;
        Input.GetAxis("Horizontal");

        

    }

    
}
