using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyController : MonoBehaviour
{
    public Keys key;

    public DescriptorDialogue descriptionWhenKeyIsPickedUp;
    public DescriptorDialogue descriptionWhenIsLooked;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }


    public void DestroyKey()
    {
        Destroy(gameObject);
    }
}
