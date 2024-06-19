using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selfdestroy : MonoBehaviour
{
    public bool hasALifeTime; 

    public float lifeTime;
    public float timeAlive;

    public GameObject[] objectsToDestroy; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime; 

        if (hasALifeTime && timeAlive >= lifeTime)
        {
            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
        }

    }

    public void Destroy()
    {
    
    Destroy(gameObject);

    }
}
