using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate;

    public TextMeshProUGUI scoreText;
    public int score = 0;

    public TextMeshProUGUI livesText;
    public int lives = 10;




    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTarget());
        UpdateScore(0);

        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {
        spawnRate = Random.Range(0, 3);
                
    }

    IEnumerator SpawnTarget()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);

            Instantiate(targets[index]);


        }
    }

    public void UpdateScore(int scoreToAdd)
    {
        //to score goes to new score count one by one insted just change the value, I make this coroutine
        StartCoroutine(scoreStep(scoreToAdd));

    }

    IEnumerator scoreStep(int scoreToAdd)
    {
        //if scoreToAdd is a positive number, this for loop will run "scoreToAdd" turns
        for (int i = 0; i < scoreToAdd; i++)
        {
            score += 1;
            scoreText.text = "Score: " + score;
            yield return new WaitForSeconds(0.05f);
        }

        //if it's a negative number it will run "scoreToAdd" turns, but will decrease score value
        for (int i = 0; i > scoreToAdd; i--)
        {
            score += -1;
            scoreText.text = "Score: " + score;
            yield return new WaitForSeconds(0.05f);
        }

    }

    public void UpdateLives(int scoreToAdd)
    {
        lives += scoreToAdd;
        livesText.text = "Lives: " + lives;
    }

}