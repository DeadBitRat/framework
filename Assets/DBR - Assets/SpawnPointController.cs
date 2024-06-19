using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpawnPointController : MonoBehaviour
{
    public List<Sprite> spriteList;

    

    // Start is called before the first frame update
    void Start()
    {
        if (Application.IsPlaying(gameObject))
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        // Choose a random sprite from the list
        int randomIndex = Random.Range(0, spriteList.Count);
        Sprite randomSprite = spriteList[randomIndex];

        // Assign the random sprite to the SpriteRenderer component
        GetComponent<SpriteRenderer>().sprite = randomSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
