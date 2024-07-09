using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBRStorySystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestFromStorySystem()
    {
        Debug.Log("This is from the Story System"); 
    }

    public void TestFromStorySystem2TheObject()
    {
        Debug.Log("This is from the Dialogue Object FUNCIONÓ!!!!");
    }

    public void FraseDePrueba(string frase)
    {
        Debug.Log("La frase de prueba es: " + frase);
    }
}
