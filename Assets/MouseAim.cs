using UnityEngine;
using System.Collections;

public class MouseAim : MonoBehaviour {


    public float coolDown = 0f;
    public float fireDelay = 0.50f;
    public GameObject bulletPrefab;
    public Vector3 bulletOffset = new Vector3(1.0f, 0.75f, 0);
    public float shootDistance = 3.0f;
    bool shotFired;

    void Update()
    {
         Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
         coolDown -= Time.deltaTime;
        
         if (Input.GetButtonUp("Fire1"))
         {
             Shoot();
             shotFired = true;
         }
        if (shotFired)
        {
            shootDistance -= Time.deltaTime;
            if (shootDistance < 0)
            {
                //Destroy(bulletPrefab);
            }
        }
    }

    void Shoot()
    {
        
        if (coolDown <= 0)
        {
            //Shoot
            Debug.Log("pew!");
            coolDown = fireDelay;

            Vector3 offSet = transform.rotation * bulletOffset;

            GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, transform.position + offSet, transform.rotation);

            bulletGo.layer = gameObject.layer;
        }
        
    }


}
