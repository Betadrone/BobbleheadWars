using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform launchPosition;
    private AudioSource audioSource;
    public bool isUpgraded;
    public float upgradeTime = 10.0f;
    private float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        currentTime += Time.deltaTime;
        if(currentTime > upgradeTime && isUpgraded == true) 
        {
            isUpgraded = false;
        }
    }

    //bullet code
    void fireBullet()
    {
        //1 - uses the bullet prefab to create an instance of it in the game.
        //GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        //2 - sets the spawn position of the created instance to the barrel of the gun.
        //bullet.transform.position = launchPosition.position;
        //3 - gives the bullet a velocity heading towards whichever direction the object is facing whom this script is attached to.
        //bullet.GetComponent<Rigidbody>().velocity = transform.parent.forward * 100;
        Rigidbody bullet = createBullet();
        bullet.velocity = transform.parent.forward * 100;

        if(isUpgraded)
        {
            Rigidbody bullet2 = createBullet();
            bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;
            Rigidbody bullet3 = createBullet();
            bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
        }

        if(isUpgraded)
        {
            audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire); // this plays shooting sound
        }
        else
        {
            audioSource.PlayOneShot(SoundManager.Instance.gunFire); // this plays shooting sound
        }
    }

    private Rigidbody createBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        bullet.transform.position = launchPosition.position;
        return bullet.GetComponent<Rigidbody>();
    }

    public void UpgradeGun()
    {
        isUpgraded = true;
        currentTime = 0;
    }
}
