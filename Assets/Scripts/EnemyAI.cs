using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    // Moving Variables
    public Transform target;
    public float speed = 3f;
    public float maxDistance;
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
    bool isGoingLeft = true;
    public float distance = 5.0f;
    bool setNewOrigin = false;
    
	// called when attached object is awake.
    void Awake()
    {
        myTransform = transform;
        origin = transform;
    }
	
	//called when application starts.
    void Start()
    {
		// set initial variables for AI.
        origin = transform;
        chase = false;
		// set the max chase distance to a random value between 3 and 10. also set the shoot distance between 1-3.
        maxDistance = Random.Range(3.0f, 10.0f);
        shootDistance = Random.Range(1.0f, 3.0f);
		//set up target for AI to chase. 
        tar = GameObject.FindGameObjectWithTag("Player");
        target = tar.transform;
    }

	// main loop for game logic.
    void Update()
    {
		// error check.
        if (target == null)
        {
            return;
        }
		// check to see if player is close enough to chase.
        if (Vector3.Distance(target.position, myTransform.position) < maxDistance)
        {
			// if so chase the player
            chase = true;
            enemyChase();
			// this sets a new origin for the AI to patrol from if it loses the player.
            setNewOrigin = true;
        }
		// if the player is beyond max chase distance stop chasing.
        if (Vector3.Distance(target.position, myTransform.position) > maxDistance)
        {
            chase = false;
        }
		// check to see if the player is close enough to shoot at.
        if(Vector3.Distance(target.position, myTransform.position) < shootDistance)
        {
            Shoot();
        }
		//set a new origin position to patrol from.
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
	
	// turn towards and chase the player.
    void enemyChase()
    {
		// turning towards.
        Vector3 dir = target.position - transform.position;

        dir.Normalize();

        float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

        Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
        
		// moving towards.
        Vector3 dir1 = target.position - myTransform.position;

        dir1.Normalize();

        myTransform.position += dir1 * speed * Time.deltaTime;
    }

    void Shoot()
    {       
        coolDown -= Time.deltaTime;
        if ( coolDown <= 0)
        {
            //Shoot
            coolDown = fireDelay;
			// make sure bullet is facing right way and ahead.
            Vector3 offSet = transform.rotation * bulletOffset;
			// actual bullet on screen
            GameObject bulletGo = (GameObject)Instantiate(bulletPrefab, transform.position + offSet, transform.rotation);

            bulletGo.layer = gameObject.layer;
        }
		// stop it forever going negative.
        if (coolDown <= 0)
        {
            coolDown = 0;
        }
        
    }
	
    void Patrol()
    {   
		// patrol left and right of origin.
        if (isGoingLeft)
        { 
			// check to see if we can turn around yet.
            if (distance < 0)
            {
                SwitchDirection();
                distance = 5;
            }
			// rotate and move. 
            Vector3 dir = (origin.position - new Vector3(distance, 0, 0)) - transform.position;

            dir.Normalize();

            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
           
            Vector3 dir1 = origin.position - myTransform.position;

            dir1.Normalize();

            myTransform.position += new Vector3(-1.0f,0,0) * speed * Time.deltaTime;
			// decrease distance by time elapsed since last frame.
            distance -= Time.deltaTime;
        }
        else
        {
			//same again but in other direction.
            if (distance < 0)
            {
                SwitchDirection();
                distance = 5;
            }
            Vector3 dir = (origin.position + new Vector3(distance,0,0)) - transform.position;

            dir.Normalize();

            float zAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;

            Quaternion desiredRot = Quaternion.Euler(0, 0, zAngle);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotationSpeed * Time.deltaTime);
            
            Vector3 dir1 = origin.position - myTransform.position;

            
            dir1.Normalize();

            
            myTransform.position += new Vector3(1.0f, 0, 0) * speed * Time.deltaTime;

            distance -= Time.deltaTime;
            

        }
    }
	
    void SwitchDirection()
    {
        isGoingLeft = !isGoingLeft;
    }

}


