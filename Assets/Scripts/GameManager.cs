using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<GameObject> targets;
    private float spawnRate;
    private int difficultyRate = 1;

    public TextMeshProUGUI scoreText;
    public int score = 0;

    public TextMeshProUGUI livesText;
    public TextMeshProUGUI gameOverText;
    public int lives = 10;

    public bool isGameActive;
    public Button restartButton;
    
    private static int highscore;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI startText;
    public GameObject titleScreen;

    private AudioSource bgAudio;
    public AudioClip bgSound;
    
    public static float bgSoundVol = 1.0f;





    // Start is called before the first frame update
    void Start()
    {
        bgAudio = GetComponent<AudioSource>();
        bgAudio.PlayOneShot(bgSound, bgSoundVol);

    }
       
    public void StartGame(int difficulty)
    {
        difficultyRate = difficulty;
        titleScreen.gameObject.SetActive(false);

        StartCoroutine(StartCountdown());

        UpdateScore(0);

        livesText.text = "Lives: " + lives;
        scoreText.text = "Score: " + score;

    }

    // Update is called once per frame
    void Update()
    {
        //spawnRate = Random.Range(0, 3) / difficultyRate;

        if (lives < 1)
        {            
            GameOver();
        }

        Debug.Log(bgSoundVol);
    }

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            spawnRate = Random.Range(0, 6) / difficultyRate;
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targets.Count);
            Debug.Log(spawnRate);
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
        if (isGameActive)
        {
            lives += scoreToAdd;
            livesText.text = "Lives: " + lives;
        }
        if (!isGameActive)
        {
            lives = 0;
        }

    }

    public void GameOver()
    {
        if (score > highscore)
        {
            highscore = score;       
            
        }

        highscoreText.text = "Highscore: " + highscore;
        gameOverText.gameObject.SetActive(true);
        highscoreText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
       
    IEnumerator StartCountdown()
    {
        startText.gameObject.SetActive(true);
        startText.text = "Ready!";
        yield return new WaitForSeconds(1);
        startText.text = "3";
        yield return new WaitForSeconds(1);
        startText.text = "2";
        yield return new WaitForSeconds(1);
        startText.text = "1";
        yield return new WaitForSeconds(1);
        startText.text = "Go!";
        yield return new WaitForSeconds(1);
        startText.gameObject.SetActive(false);
        isGameActive = true;
        StartCoroutine(SpawnTarget());
    }
}