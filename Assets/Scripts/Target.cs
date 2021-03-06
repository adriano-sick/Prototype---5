﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private float destroyDelay = 0.1f;
    private Rigidbody targetRb;
    private GameManager gameManager;
    private AudioSource targetAudio;

    public float minSpeed = 12f;
    public float maxSpeed = 16f;
    public float xRange = 4f;
    public float ySpawnPos = -1f;
    public float maxTorque = 8f;
    public static float SFXVol = 1f;
    public int pointValue = 0;
    public int loseLiveValue = -1;
    public int indexParticles;
    public AudioClip clickSound;
    public ParticleSystem[] explosionParticles;
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
        if (gameManager.isGameActive && !gameManager.isGamePaused)
        {
            if (!gameObject.CompareTag("Bad"))
            {
                gameManager.UpdateScore(pointValue);
                targetAudio.PlayOneShot(clickSound, SFXVol);

                Instantiate(explosionParticles[indexParticles], transform.position, transform.rotation);
                Destroy(gameObject, destroyDelay);
            }

            if (gameObject.CompareTag("Bad"))
            {
                gameManager.UpdateLives(loseLiveValue);
                targetAudio.PlayOneShot(clickSound, SFXVol);

                Instantiate(explosionParticles[indexParticles], transform.position, transform.rotation);
                Destroy(gameObject, destroyDelay);
            }
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!gameObject.CompareTag("Bad"))
        {
            gameManager.UpdateLives(loseLiveValue);            
            Destroy(gameObject);
        }
        else if(gameObject.CompareTag("Bad"))
        {
            Destroy(gameObject);
        }        
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