using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float destructTime = 3.0f;

    public void Initiate()
    {
        Invoke("selfDestruct", destructTime);
    }

    private void selfDestruct()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
