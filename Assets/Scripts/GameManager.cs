using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;
    public Text scoreText, gameOverScoreText;
    static int score;
    public Canvas MenuCanvas, MainGameCanvas, GameOverCanvas;
    public GameObject standardSpawnerP, advancedSpawnerP;
    public Spawner[] standardSpawners;
    public AdvanceSpawner[] advancedSpawners;
    public GameObject pauseButton, pauseScreen;
    public GameObject[] players;
    public AudioSource bgAudioSource;
    public static GameMode gameMode;
    public float timeLevel = 200f, objectSpeed, decreaseTimeBy, difficultyTime = 40f, playerSpeed = 10f;
    public float[] spawnTime;
    public static bool isGamePlaying = false, defPos = true, isPaused;
    public Animator animator;
    public PlayerMovement player1Movement, player2Movement;
    public float musicVol, sfxVol;
    public Toggle musicToggle, sfxToggle;
    public Toggle musicMainToggle, sfxMainToggle;
    public AudioSource musicAudioSource;
    public AudioSource[] sfxAudioSources;
    public AudioSource audioSourceGM;
    public AudioClip gameOverFX, buttonClick;
    public Animator canvasAnimator;
    public TextMeshProUGUI highscore;

    private bool[] shouldSpawn;
    private float spawnTimeMax;

    public static float variableObjectSpeed;

    void Awake()
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
        shouldSpawn = new bool[2];
        shouldSpawn[0] = false;
        shouldSpawn[1] = false;
        player1Movement = players[1].GetComponent<PlayerMovement>();
        player2Movement = players[0].GetComponent<PlayerMovement>();
        musicVol = PlayerPrefs.GetInt("music", 1);
        if (musicVol == 1)
        {
            musicToggle.isOn = true;
            musicMainToggle.isOn = true;
            musicAudioSource.volume = 1f;
        } else
        {
            musicToggle.isOn = false;
            musicMainToggle.isOn = false;
            musicAudioSource.volume = 0f;
        }
        sfxVol = PlayerPrefs.GetInt("sfx", 1);
        if (musicVol == 1)
        {
            sfxToggle.isOn = true;
            sfxMainToggle.isOn = true;
            foreach (AudioSource audioSource in sfxAudioSources)
                audioSource.volume = 1f;
        }
        else
        {
            sfxToggle.isOn = false;
            sfxMainToggle.isOn = false;
            foreach (AudioSource audioSource in sfxAudioSources)
                audioSource.volume = 0f;
        }
        audioSourceGM = GetComponent<AudioSource>();
        highscore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
    }

    public void ChangeMusicVol()
    {
        if (musicToggle.isOn)
        {
            musicVol = 1f;
            PlayerPrefs.SetInt("music", 1);
        } else
        {
            musicVol = 0f;
            PlayerPrefs.SetInt("music", 0);
        }
        musicMainToggle.isOn = musicToggle.isOn;
        musicAudioSource.volume = musicVol;
    }

    public void ChangeSFXVol()
    {
        if (sfxToggle.isOn)
        {
            sfxVol = 1f;
            PlayerPrefs.SetInt("sfx", 1);
        }
        else
        {
            sfxVol = 0f;
            PlayerPrefs.SetInt("sfx", 0);
        }
        sfxMainToggle.isOn = sfxToggle.isOn;
        foreach (AudioSource audioSource in sfxAudioSources)
            audioSource.volume = sfxVol;
    }

    public void ToggleMusic()
    {
        if (musicMainToggle.isOn)
        {
            musicVol = 1f;
            PlayerPrefs.SetInt("music", 1);
        }
        else
        {
            musicVol = 0f;
            PlayerPrefs.SetInt("music", 0);
        }
        musicToggle.isOn = musicMainToggle.isOn;
        musicAudioSource.volume = musicVol;
    }

    public void ToggleSFX()
    {
        if (sfxMainToggle.isOn)
        {
            sfxVol = 1f;
            PlayerPrefs.SetInt("sfx", 1);
        }
        else
        {
            sfxVol = 0f;
            PlayerPrefs.SetInt("sfx", 0);
        }
        sfxToggle.isOn = sfxMainToggle.isOn;
        foreach (AudioSource audioSource in sfxAudioSources)
            audioSource.volume = sfxVol;
    }

    private void Start()
    {
        //ShowMenu();

        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }
    }


    public void ShowMenu()
    {
        //PlayBtnClickSound();
        highscore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        if (isPaused)
        {
            isPaused = false;
            canvasAnimator.SetTrigger("PauseMenuFadeOut");
            canvasAnimator.SetTrigger("MainMenuFadeIn");
        } else
        {
            canvasAnimator.SetTrigger("GameOverFadeOut");
            canvasAnimator.SetTrigger("MainMenuFadeIn");
        }
        //MainGameCanvas.gameObject.SetActive(false);
        //GameOverCanvas.gameObject.SetActive(false);
        //MenuCanvas.gameObject.SetActive(true);
        DestroyObjects();
        DisableGame();
    }

    public void DestroyObjects()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        GameObject[] advanceObstacles = GameObject.FindGameObjectsWithTag("AdvancedObstacle");
        GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
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
        canvasAnimator.SetTrigger("MenuSlideOut");
        score = 0;
        scoreText.text = score.ToString();
        if (GameOverCanvas.gameObject.activeInHierarchy)
        {
            canvasAnimator.SetTrigger("GameOverFadeOut");
        }
        canvasAnimator.SetTrigger("MainGameCanvasSlideIn");
        StartCoroutine(WaitForAnimEnd(2f, 0));
        player1Movement.Reset();
        player2Movement.Reset();
        foreach (GameObject player in players)
        {
            player.SetActive(true);
        }
    }

    IEnumerator WaitForAnimEnd(float sec, int caseNum)
    {
        yield return new WaitForSeconds(sec);

        switch (caseNum)
        {
            case 0:
                {
                    isGamePlaying = true;
                    gameMode = GameMode.standard;
                    bgAudioSource.Play();
                    //MainGameCanvas.gameObject.SetActive(true);
                    GameOverCanvas.gameObject.SetActive(false);
                    //MenuCanvas.gameObject.SetActive(false);
                    standardSpawnerP.SetActive(true);
                    advancedSpawnerP.SetActive(true);
                    Time.timeScale = 1f;
                    StartGame();
                    shouldSpawn[0] = true;
                    shouldSpawn[1] = true;
                    canvasAnimator.ResetTrigger("MenuSlideOut");
                    canvasAnimator.ResetTrigger("GameOverFadeOut");
                }
                break;
            case 1:
                {
                    isPaused = false;
                    canvasAnimator.ResetTrigger("PauseMenuFadeOut");
                }
                break;
        }
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
        variableObjectSpeed = objectSpeed;
        timeLevel += Time.time;
        spawnTimeMax = spawnTime[1];
        InvokeRepeating("IncreaseDifficulty", 1f, difficultyTime);
        isPaused = false;
    }
    void IncreaseDifficulty()
    {
        //if (spawnTimeMax - decreaseTimeBy > spawnTime[0]) spawnTimeMax -= decreaseTimeBy;
        variableObjectSpeed += 1f;
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
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
            return;
        }
        if (Input.GetKeyDown("a"))
        {
            player1Movement.OnClick();
        }
        if (Input.GetKeyDown("d"))
        {
            player2Movement.OnClick();
        }

        if (!isPaused)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                Vector3 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    if (!(touchPos.x <= -1.55 && touchPos.x >= -2.5 && touchPos.y >= 4 && touchPos.y <= 5))
                    {
                        if (Camera.main.ScreenToWorldPoint(touch.position).x > 0)
                        {
                            player2Movement.OnClick();
                        }
                        else
                        {
                            player1Movement.OnClick();
                        }
                    }
                }

                if (Input.touchCount > 1)
                {
                    Touch touch2 = Input.GetTouch(1);

                    touchPos = Camera.main.ScreenToWorldPoint(touch2.position);

                    if (touch2.phase == TouchPhase.Began)
                    {
                        if (!(touchPos.x <= -1.55 && touchPos.x >= -2.5 && touchPos.y >= 4 && touchPos.y <= 5))
                        {
                            if (Camera.main.ScreenToWorldPoint(touch2.position).x > 0)
                            {
                                player2Movement.OnClick();
                            }
                            else
                            {
                                player1Movement.OnClick();
                            }
                        }
                    }
                }
            }
        }

        if (!isPaused)
        {
            if (shouldSpawn[0])
            {
                shouldSpawn[0] = false;
                StartCoroutine(WaitBeforeSpawnForOne());
            }
            if (shouldSpawn[1])
            {
                shouldSpawn[1] = false;
                StartCoroutine(WaitBeforeSpawnForTwo());
            }
        }
    }

    void SpawnStandard(int x)
    {

        if (x == 0)
        {
            standardSpawners[0].SpawnObject(variableObjectSpeed);
        }
        else
        {
            standardSpawners[1].SpawnObject(variableObjectSpeed);
        }
    }

    public void PauseGame()
    {
        if (isGamePlaying)
        {
            PlayBtnClickSound();
            isPaused = true;
            canvasAnimator.SetTrigger("MainGameSlideOut");
            canvasAnimator.SetTrigger("PauseMenuFadeIn");
            StartCoroutine(WaitForAnimEnd(2, 10));
        }
    }

    public void UnpauseGame()
    {
        //PlayBtnClickSound();
        canvasAnimator.SetTrigger("PauseMenuFadeOut");
        canvasAnimator.SetTrigger("MainGameCanvasSlideIn");
        StartCoroutine(WaitForAnimEnd(2, 1));
    }
    public void GameOver()
    {
        audioSourceGM.clip = gameOverFX;
        audioSourceGM.Play();
        CancelInvoke();
        isGamePlaying = false;
        //Time.timeScale = 0;
        canvasAnimator.SetTrigger("MainGameSlideOut");
        canvasAnimator.SetTrigger("GameOverFadeIn");
        //MainGameCanvas.gameObject.SetActive(false);
        //GameOverCanvas.gameObject.SetActive(true);
        gameOverScoreText.text = score.ToString();
        if (score > PlayerPrefs.GetInt("highscore", 0))
            PlayerPrefs.SetInt("highscore", score);
        //MenuCanvas.gameObject.SetActive(false);
        DisableGame();
        DestroyObjects();
        shouldSpawn[0] = false;
        shouldSpawn[1] = false;
        StopAllCoroutines();
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

    IEnumerator WaitBeforeSpawnForOne()
    {
        yield return new WaitForSeconds(Random.Range(spawnTime[0], spawnTimeMax));

        SpawnStandard(0);

        shouldSpawn[0] = true;
    }

    IEnumerator WaitBeforeSpawnForTwo()
    {
        yield return new WaitForSeconds(Random.Range(spawnTime[0], spawnTimeMax));

        SpawnStandard(1);

        shouldSpawn[1] = true;
    }

}

public enum GameMode { standard, advanced, transitionToAdvanced, transitionToStandard };

