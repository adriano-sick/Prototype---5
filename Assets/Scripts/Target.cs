using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float minSpeed = 12f;
    public float maxSpeed = 16f;

    public float xRange = 4f;
    public float ySpawnPos = -6f;
        
    public float maxTorque = 10f;

    private Rigidbody targetRb;

    private GameManager gameManager;

    public int pointValue = 0;
    public int loseLiveValue = -1;

    private AudioSource targetAudio;
    public AudioClip clickSound;
    private float destroyDelay = 0.1f;
    public float SFXVol = 1f;

    public ParticleSystem[] explosionParticles;
    public int indexParticles;


    // Start is called before the first frame update
    void Start()
    {
        targetRb = GetComponent<Rigidbody>();
        targetRb.AddForce(RandomUpwardForce(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();

        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        targetAudio = GetComponent<AudioSource>();
        indexParticles = Random.Range(0, explosionParticles.Length);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        gameManager.UpdateScore(pointValue);
        targetAudio.PlayOneShot(clickSound, SFXVol);

        Instantiate(explosionParticles[indexParticles], transform.position, transform.rotation);
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.UpdateLives(loseLiveValue);        
        Destroy(gameObject);
    }

    float RandomTorque()
    {
        float randomTorque = Random.Range(0, maxTorque);
        return randomTorque;
    }

    Vector3 RandomUpwardForce()
    {
        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        return Vector3.up * randomSpeed;
    }

    Vector3 RandomSpawnPos()
    {       

        float randomXPos = Random.Range(-xRange, xRange);
        return new Vector3(randomXPos, ySpawnPos);
    }

    


}