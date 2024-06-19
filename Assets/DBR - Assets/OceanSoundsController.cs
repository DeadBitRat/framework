using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanSoundsController : MonoBehaviour
{
    public AudioClip[] oceanWaveSounds;
    public float interval = 5.0f; // Interval between playing clips in seconds
    public float startDelay;

    public float minWavesVolume = 0.01f; // Minimum volume for the sound effect
    public float maxWavesVolume = 0.02f; // Maximum volume for the sound effect

    public AudioSource oceanWavesSource;

    public bool oceanWavesSoundsActive;
    public bool activeOceanWaves; 
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!activeOceanWaves)
        {
            Invoke("StartOceanSoundsCoroutine", startDelay);
            activeOceanWaves = true;
        }
    }

    IEnumerator PlayRandomOceanWaves()
    {
        while (oceanWavesSoundsActive)
        {
            yield return new WaitForSeconds(interval);

            if (oceanWaveSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, oceanWaveSounds.Length);

                
                float randomVolume = Random.Range(minWavesVolume, maxWavesVolume);
                oceanWavesSource.volume = randomVolume;
                oceanWavesSource.PlayOneShot(oceanWaveSounds[randomIndex]);
            }
            else
            {
                Debug.LogError("No ocean wave sounds assigned.");
            }
        }
    }

    void StartOceanSoundsCoroutine()
    {
        StartCoroutine(PlayRandomOceanWaves());
    }
}
