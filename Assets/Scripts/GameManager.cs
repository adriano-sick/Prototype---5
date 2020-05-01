using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{    
    
    private static int highscore;
    private int difficultyRate = 1;
    private float spawnRate;
    private AudioSource bgAudio;

    public bool isGameActive = false;
    public bool isGamePaused = false;
    public int score = 0;
    public int lives = 10;
    public static float bgSoundVol = 1.0f;    
    public TextMeshProUGUI startText;
    public TextMeshProUGUI pauseText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;    
    public TextMeshProUGUI highscoreText;
    public Button restartButton;
    public Slider mainSlider;
    public GameObject titleScreen;
    public List<GameObject> targets;    

    // Start is called before the first frame update
    void Start()
    {
        bgAudio = GetComponent<AudioSource>();        
        mainSlider.value = bgSoundVol; 
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
        if (lives < 1)
        {            
            GameOver();
        }

        bgSoundVol = bgAudio.volume;

        if (Input.GetKeyDown(KeyCode.Return) && isGameActive)
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                bgAudio.Pause();
                isGamePaused = true;
                pauseText.gameObject.SetActive(true);
                restartButton.gameObject.SetActive(true);
            }
            else if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
                bgAudio.Play();
                isGamePaused = false;
                pauseText.gameObject.SetActive(false);
                restartButton.gameObject.SetActive(false);
            }
            
        }
    }
    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            spawnRate = Random.Range(0, 6) / difficultyRate;
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