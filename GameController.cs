using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Settings")]  /********/
    [SerializeField]
    [Range(0.1f, 2f)]
    float restartDelayDuration=0.5f;
    [SerializeField] AudioClip pauseSound;
    [SerializeField] AudioClip gameOverSound;


    [Header("Data")]    /********/
    private static GameController I;
    public static GameController Instance { get { return I; } }
    bool gameActive = true;
    bool isPaused = false;
    bool restarting = false;
    float restartDelayTimer;



    [Header("Components")]    /********/
    public GameObject player;
    [SerializeField]
    GameObject pauseObject;
    public GameObject musicObject;
    [SerializeField]
     AudioLowPassFilter lowPassFilter;
    [SerializeField]
    GameObject gameOverObject;
    [SerializeField] Animator screenFiltersAnim;
    [SerializeField] AudioSource aS;

    #region Default Methods
    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            I = this;
        }
    }


    void Update()
    {
        RestartDelayTimer();
    }
    #endregion

    #region Unique Methods

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetMusic()
    {
        PlayerPrefs.SetFloat("musicTime", 0f);
    }
    public void Pause()
    {
        if (!isPaused)
        {
            aS.clip = pauseSound;
            aS.Play();
            pauseObject.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
            lowPassFilter.cutoffFrequency = 1702f;
        }
        else
        {
            pauseObject.SetActive(false);
            Time.timeScale = 1f;
            isPaused = false;
            lowPassFilter.cutoffFrequency = 5007.7f;
        }
        
    }

    public void LoadScene()
    {
        restartDelayTimer = restartDelayDuration;
        restarting = true;
        screenFiltersAnim.Play("blackFadeIn");
    }

    public void QuitGame()
    {
        PlayerPrefs.SetFloat("musicTime", 0f);
        Application.Quit();

    }

    public void GameOver()
    {
        if (gameActive)
        {
            gameActive = false;
            aS.clip = gameOverSound;
            aS.Play();
            gameOverObject.SetActive(true);
        }

    }

    void RestartDelayTimer()
    {
        if (restarting)
        {
            if (restartDelayTimer > 0) restartDelayTimer -= Time.unscaledDeltaTime;
            else RestartScene();
        }
    }

    void RestartScene()
    {
        restartDelayTimer = 0f;
        restarting = false;
        Time.timeScale = 1f;
        lowPassFilter.cutoffFrequency = 5007.7f;
        PlayerPrefs.SetFloat("musicTime", musicObject.GetComponent<AudioSource>().time);
        SceneManager.LoadScene("Game");
    }

    #endregion


}

