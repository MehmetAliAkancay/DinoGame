using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverText;
    public Button retryButton; 
    private Player player;
    private ObstacleSpawner obstacleSpawner;
    private float score;
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    private void Awake() {
        if(Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);
    }
    private void Start() {
        player = FindObjectOfType<Player>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        NewGame();
    }
    private void Update() {
        gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
    }
    private void OnDestroy() {
        if(Instance == this)
            Instance = null;
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();
        foreach(var obj in obstacles){
            Destroy(obj.gameObject);
        }
        score = 0;
        gameSpeed = initialGameSpeed;
        player.gameObject.SetActive(true);
        obstacleSpawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        UpdateHighScore();
    }
    public void GameOver()
    {
        gameSpeed = 0;
        enabled = false;
        player.gameObject.SetActive(false);
        obstacleSpawner.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        UpdateHighScore();
    }
    private void UpdateHighScore(){
        float highscore = PlayerPrefs.GetFloat("HighScore");
        if(score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat("HighScore",highscore);
            highScoreText.text = Mathf.FloorToInt(highscore).ToString("D5");
        }
    }

}