using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Image lifeBomb1; 
    [SerializeField] private Image lifeBomb2; 
    [SerializeField] private Image lifeBomb3; 
    [SerializeField] public int hudLives;

    [SerializeField] private Image messageQuest; 
    [SerializeField] private Image messageWarning; 
    [SerializeField] private Image windowWarning1; 
    [SerializeField] private Image windowWarning2;


    [SerializeField] private LifeAndDeath lifeAndDeath;

    [SerializeField] private float timeRemaining = 3.5f;
    public bool timerIsRunning = false;
    [SerializeField] private int timeBeforeApparition = 0;

    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip wakeUpSound;
    public AudioClip errorSound;

    void Awake()
    {
        messageQuest.enabled = false;
        windowWarning1.enabled = false;
        windowWarning2.enabled = false;
        audioSource.clip = wakeUpSound;
        audioSource.Play();
        audioSource2.clip = errorSound;
        audioSource2.Play();

    }

    // Start is called before the first frame update
    private void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                if(timeBeforeApparition > 10)
                {
                    windowWarning1.enabled = true;

                    if (timeBeforeApparition > 50)
                    {
                        windowWarning2.enabled = true;
                    }
                    else
                    {
                        timeBeforeApparition +=1;
                    }
                }else{
                    timeBeforeApparition +=1;
                }
            }
            else
            {
                messageWarning.enabled = false;
                windowWarning1.enabled = false;
                windowWarning2.enabled = false;

                timeRemaining = 0;
                timerIsRunning = false;
                messageQuest.enabled = true;
                audioSource2.Stop() ;
            }
        }
        
        if(Input.GetAxisRaw("Horizontal")!= 0f)
        {
            messageWarning.enabled = false;
            messageQuest.enabled = false;
            audioSource2.Stop();

        }

        hudLives = lifeAndDeath.updateLives;
        LifeBar();
    }

    private void LifeBar ()
    {
        if (hudLives == 3)
        {
            lifeBomb3.enabled = true;
            lifeBomb2.enabled = true;
        }

        if(hudLives < 3 )
        {
            lifeBomb3.enabled = false;
            if(hudLives<2)
            {
                lifeBomb2.enabled = false;
                if(hudLives<1)
                {
                    lifeBomb1.enabled = false;
                }
            }
        }
    }
}
