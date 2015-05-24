using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    // Moving Variables
    public Transform target;
    public float speed = 3f;
    public int maxDistance;
    public Transform myTransform;
    public bool chase;
    public float rotationSpeed = 180.0f;
    GameObject tar;

    // Shooting variables
    public float coolDown = 0f;
    public float fireDelay = 0.50f;
    public GameObject bulletPrefab;
    public Vector3 bulletOffset = new Vector3(0.75f, 0.75f, 0);
    public float shootDistance = 3.0f;

    // Patrol variables
    public Transform origin;
    bool isGoingLeft = false;
    float distance = 5.0f;
    bool setNewOrigin = false;
    bool tileCol = false;

    void Awake()
    {
        myTransform = transform;
        origin = transform;
    }

    void Start()
    {
        chase = false;
        maxDistance = 5;
        tar = GameObject.FindGameObjectWithTag("Player");
        target = tar.transform;
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }
        if (Vector3.Distance(target.position, myTransform.position) < maxDistance)
        {
            chase = true;
            enemyChase();
            setNewOrigin = true;
        }
        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
        {
            chase = false;
        }
        if(Vector3.Distance(target.position, myTransform.position) < shootDistance)
        {
            Shoot();
        }
        if (!chase)
        {
            if (setNewOrigin)
            {
                origin = transform;
                setNewOrigin = false;
            }
            Patrol();
        }

    }

    void enemyChase()
    {
        Vector3 dir = target.position - transform.position;

        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
        // Get a direction vector from us to the target
        Vector3 dir1 = target.position - myTransform.position;

        // Normalize it so that it's a unit direction vector
        dir1.Normalize();

        // Move ourselves in that direction
        myTransform.position += dir1 * speed * Time.deltaTime;
    }

    void Shoot()
    {       
        coolDown -= Time.deltaTime;
        if ( coolDown <= 0)
        {
            //Shoot
            Debug.Log("pew!");
            coolDown = fireDelay;

            Vector3 offSet = transform.rotation * bulletOffset;

            GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, transform.position + offSet, transform.rotation);

            bulletGo.layer = gameObject.layer;
        }
        if (coolDown <= 0)
        {
            coolDown = 0;
        }
        
    }

    void Patrol()
    {
        float distFromStart = transform.position.x - origin.position.x;   

        if (isGoingLeft)
        { 
            if (distFromStart < -distance)
            {
                SwitchDirection();
            }
            Vector3 dir = (origin.position - new Vector3(distance, 0, 0)) - transform.position;

            dir.Normalize();

            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
           
            Vector3 dir1 = origin.position - myTransform.position;

            dir1.Normalize();

            myTransform.position += new Vector3(-1.0f,0,0) * speed * Time.deltaTime;
            if (tileCol)
            {
                SwitchDirection();
            }
        }
        else
        {
            if (distFromStart > distance)
            {
                SwitchDirection();
            }
            Vector3 dir = (origin.position + new Vector3(distance,0,0)) - transform.position;

            dir.Normalize();

            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
            
            Vector3 dir1 = origin.position - myTransform.position;

            
            dir1.Normalize();

            
            myTransform.position += new Vector3(1.0f, 0, 0) * speed * Time.deltaTime;
            if (tileCol)
            {
                SwitchDirection();
            }
        }
    }


    void SwitchDirection()
    {
        isGoingLeft = !isGoingLeft;
    }

}


