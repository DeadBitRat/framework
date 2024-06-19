using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.Windows;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private InputDetector inputDetector; 

    public float maxHealth;
    public float currentHealth;

    public Selfdestroy destroy;
    public Animator animator; 

    public bool isGettingHurt;
    public bool isHurt = false; 
    public bool isRecovering;
    public bool canGetHurt = true; 

    public float recoveringTime = 0.25f; 

    
    public HurtAndRecoverAnimations[] hurtAndRecoverAnimations;
    private int randomIndex; 

    

    // Start is called before the first frame update
    void Start()
    {
        destroy = GetComponent<Selfdestroy>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0f)
        {
            Debug.Log("Me Muriceo");
            destroy.Destroy();
        }


        #region Animations

        animator.SetBool("GettingHurt", isGettingHurt);
        animator.SetBool("IsHurt", isHurt);
        animator.SetBool("Recovering", isRecovering); 

        #endregion

       

    }

    public void ReceivingDamage(float damage, float damageDuration)
    {   
        if (canGetHurt) {
            canGetHurt = false;
            inputDetector.SwitchInputOff(); 
        currentHealth -= damage;

        if (currentHealth >= 0f) { 
        StartCoroutine(SetReceivedDamageTrueForDuration(damageDuration));

            // Check if the array is not empty
            if (hurtAndRecoverAnimations.Length != 0 && !isHurt)
            {
                SelectRandomIndex();  // Va a correr 1 sola vez. 
            

            // Get a random index within the bounds of the array
            

                // Return the randomly selected AnimationClip

                animator.Play(hurtAndRecoverAnimations[randomIndex].gettingHurtAnimation.name);
                isHurt = true; 
                
                    
            }

            else
            {
                Debug.LogWarning("The gettingHurtAnimations array is empty!");
                return;
            }


        }
        }
    }

    public void Healing(float heal)
    {
        currentHealth += heal;
    }

    public IEnumerator SetReceivedDamageTrueForDuration(float duration)
    {
        isGettingHurt = true;

        // Esperar la duración especificada
        yield return new WaitForSeconds(duration);

        // Después de la duración, establecer receivedDamage como falso
        isGettingHurt = false;
        StartCoroutine(SetIsRecoveringTrueForDuration(recoveringTime)); 
    }

    public IEnumerator SetIsRecoveringTrueForDuration(float duration)
    {
        isRecovering = true;
        animator.Play(hurtAndRecoverAnimations[randomIndex].recoverAnimation.name);
        // Esperar la duración especificada
        yield return new WaitForSeconds(duration);

        // Después de la duración, establecer receivedDamage como falso
        isHurt = false;
        isRecovering = false;
        inputDetector.SwitchInputOn();
        canGetHurt = true; 

    }

    public void SelectRandomIndex()
    {
        randomIndex = Random.Range(0, hurtAndRecoverAnimations.Length);
    }



    [System.Serializable]
    public class HurtAndRecoverAnimations
    {
        public AnimationClip gettingHurtAnimation;
        public AnimationClip recoverAnimation; 
    }

}
