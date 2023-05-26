using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeAndDeath : MonoBehaviour
{
    [SerializeField] private int lives = 3;
    [SerializeField] public int updateLives ;
    [SerializeField] private float respawnTime = 2.5f;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] public Transform lastCheckPointPos;
    [SerializeField] private MonoBehaviour playerControlerScript;

    [SerializeField] public bool isRegen;



    private Rigidbody2D rb;

    // Start is called before the first frame update
    public void Awake()
    {

    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        updateLives = lives;
        lastCheckPointPos = GameObject.Find("InitialSpawnPoint").transform;

        Spawn();
    }
    
    void FixedUpdate()
    {
        if (isRegen)
        {
            updateLives = lives;
            isRegen = false;
        }

    }

    public void DoDie()
    {
        updateLives -= 1;
        if(updateLives < 1)
        {
            // GAME OVER !
            //print("GAME OVER !");
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);

        }
        else
        {
            playerControlerScript.enabled = false;
            Invoke("Spawn", respawnTime);
            Explosion.isActive = false;

        }
    }

    public void Spawn()
    {
        
        spawnPoint.transform.position = lastCheckPointPos.transform.position;
        playerControlerScript.enabled = true;
        rb.velocity = Vector2.zero;
        transform.position = spawnPoint.position;
    }


}
