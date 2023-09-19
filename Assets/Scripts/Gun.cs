using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform launchPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!IsInvoking("fireBullet"))
            {
                InvokeRepeating("fireBullet", 0.0f, 0.1f);
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            CancelInvoke("fireBullet");
        }
    }

    //bullet code
    void fireBullet()
    {
        //1 - uses the bullet prefab to create an instance of it in the game.
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        //2 - sets the spawn position of the created instance to the barrel of the gun.
        bullet.transform.position = launchPosition.position;
        //3 - gives the bullet a velocity heading towards whichever direction the object is facing whom this script is attached to.
        bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
    }
}
