using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour
{
    public Transform target;
    public float navigationUpdate;
    private float navigationTime = 0;
    private NavMeshAgent agent;
    public UnityEvent OnDestroy;

    public Rigidbody head;
    public bool isAlive = true;
    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            if(isAlive)
            {
                navigationTime += Time.deltaTime;
                if (navigationTime > navigationUpdate)
                {
                    agent.destination = target.position;
                    navigationTime = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(isAlive)
        {
            Die();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);            
        }
    }

    public DeathParticles GetDeathParticles()
    {
        if (deathParticles == null)
        {
            deathParticles = GetComponentInChildren<DeathParticles>();
        }
        return deathParticles;
    }

    public void Die()
    {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 26.0f, 3.0f);

        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        
        head.GetComponent<SelfDestruct>().Initiate();
        if (deathParticles)
        {
            deathParticles.transform.parent = null;
            deathParticles.Activate();
        }
        Destroy(gameObject);
    }
}
