using UnityEngine;
using System.Collections;

public class BossDamage : MonoBehaviour
{

    public int bhealth = 1;
    public float binvuln = 0f;
    int bcorrectlayer;
    

    void Start()
    {
        bcorrectlayer = gameObject.layer;
        
    }

    void OnTriggerEnter2D()
    {
        Debug.Log("Trigger!");

        bhealth--;
        binvuln = 2f;
        gameObject.layer = 10;
    }

    void Update()
    {

        binvuln -= Time.deltaTime;

        if (bhealth == 0)
        {
            bdie();
        }
        if (binvuln <= 0)
        {
            gameObject.layer = bcorrectlayer;
            binvuln = 0;
        }
    }

    void bdie()
    {
        Destroy(gameObject);
        
    }
}
