using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance=null;
    public Text scoreText,gameOverScoreText;
    static int score;
    public Canvas MenuCanvas, MainGameCanvas, GameOverCanvas;
    public GameObject standardSpawnerP, advancedSpawnerP;
    public Spawner[] standardSpawners;
    public AdvanceSpawner[] advancedSpawners;
    public GameObject pauseButton,pauseScreen;
    public GameObject[] players;
    public AudioSource bgAudioSource;
    public static GameMode gameMode;
    public float timeLevel = 200f, objectSpeed = 7f, spawnTime = 1f,difficultyTime=40f,playerSpeed=10f;
    public static bool isGamePlaying = false,defPos=true;
    public Animator animator;
    PlayerMovement player1Movement, player2Movement;
    public float musicVol, sfxVol;
    public Slider musicSlider, sfxSlider;
    public AudioSource musicAudioSource;
    public AudioSource[] sfxAudioSources;
    public AudioSource audioSourceGM;
    public AudioClip gameOverFX, buttonClick;
    void Awake ()
    {
        score = 0;
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);

        standardSpawners = standardSpawnerP.GetComponentsInChildren<Spawner>();
        advancedSpawners = advancedSpawnerP.GetComponentsInChildren<AdvanceSpawner>();
        players = GameObject.FindGameObjectsWithTag("Player");
        player1Movement = players[1].GetComponent<PlayerMovement>();
        player2Movement = players[0].GetComponent<PlayerMovement>();
        musicVol = musicSlider.value;
        sfxVol = sfxSlider.value;
        audioSourceGM = GetComponent<AudioSource>();
    }

    public void ChangeMusicVol()
    {
        musicVol = musicSlider.value / 100;
        musicAudioSource.volume = musicVol;
    }

    public void ChangeSFXVol()
    {
        sfxVol = sfxSlider.value / 100;
        foreach (AudioSource audioSource in sfxAudioSources)
            audioSource.volume = sfxVol;
    }

    private void Start()
    {
        ShowMenu();
    }
    
    
    public void ShowMenu()
    {
        //PlayBtnClickSound();
        MainGameCanvas.gameObject.SetActive(false);
        GameOverCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(true);
        DestroyObjects();
        DisableGame();
    }

    public void DestroyObjects()
    {
        GameObject[] obstacles= GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] advanceObstacles = GameObject.FindGameObjectsWithTag("AdvancedObstacle");
        GameObject[] collectibles= GameObject.FindGameObjectsWithTag("Collectible");
        foreach (GameObject gameObject in obstacles)
        {
            Destroy(gameObject);
        }
        foreach (GameObject gameObject in collectibles)
        {
            Destroy(gameObject);
        }
        foreach (GameObject gameObject in advanceObstacles)
        {
            Destroy(gameObject);
        }
    }

    public void PlayGame()
    {
        PlayBtnClickSound();
        isGamePlaying = true;
        gameMode = GameMode.standard;
        bgAudioSource.Play();
        score = 0;
        scoreText.text = score.ToString();
        MainGameCanvas.gameObject.SetActive(true);
        GameOverCanvas.gameObject.SetActive(false);
        MenuCanvas.gameObject.SetActive(false);
        standardSpawnerP.SetActive(true);
        advancedSpawnerP.SetActive(true);
        foreach(GameObject player in players)
        player.SetActive(true);
        Time.timeScale = 1f;
        StartGame();
    }

    public void PlayBtnClickSound()
    {
        audioSourceGM.clip = buttonClick;
        audioSourceGM.Play();
    }

    void StartGame()
    {
        pauseButton.SetActive(true);
        defPos = true;
        objectSpeed = 7f;
        spawnTime = 1f;
        timeLevel += Time.time;
        InvokeRepeating("SpawnStandard", 2f, spawnTime);
        InvokeRepeating("IncreaseDifficulty", 1f, difficultyTime);
    }
    void IncreaseDifficulty()
    {
        if (spawnTime - 0.1f > 0) spawnTime -= 0.1f;
        objectSpeed += 1f;
    }
    void DisableGame()
    {
        isGamePlaying = false;
        bgAudioSource.Stop();
        MainGameCanvas.gameObject.SetActive(false);
        standardSpawnerP.SetActive(false);
        advancedSpawnerP.SetActive(false);
        foreach (GameObject player in players)
            player.SetActive(false);
        pauseScreen.SetActive(false);
    }

    public void Update()
    {
        if (!isGamePlaying)
            return;
        if (Input.GetKeyDown("a"))
        {
            player1Movement.OnClick();
        }
        if (Input.GetKeyDown("d"))
        {
            player2Movement.OnClick();
        }
    }

    void SpawnStandard()
    {
        
        if (Random.Range(0, 2)==0)
        {
            standardSpawners[0].SpawnObject(objectSpeed);
        }
        else
        {
            standardSpawners[1].SpawnObject(objectSpeed);
        }
    }

    public void PauseGame()
    {
        PlayBtnClickSound();
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void UnpauseGame()
    {
        //PlayBtnClickSound();
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        pauseScreen.SetActive(false);
    }
    public void GameOver()
    {
        audioSourceGM.clip = gameOverFX;
        audioSourceGM.Play();
        CancelInvoke();
        isGamePlaying = false;
        Time.timeScale = 0;
        MainGameCanvas.gameObject.SetActive(false);
        GameOverCanvas.gameObject.SetActive(true);
        gameOverScoreText.text = score.ToString();
        MenuCanvas.gameObject.SetActive(false);
        DisableGame();
        DestroyObjects();
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public static GameMode GetGameMode()
    {
        return gameMode;
    }

    public static void ChangeGameMode(GameMode newGameMode)
    {
        Debug.Log("changing game mode to " + newGameMode);
        gameMode = newGameMode;
    }
}

public enum GameMode {standard, advanced,transitionToAdvanced,transitionToStandard};
