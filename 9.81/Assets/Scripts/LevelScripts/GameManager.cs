using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int level = 0;
    private int enemies = 0;

    [Header("Audio")]
    [SerializeField]
    private AudioClip levelTransition;
    [SerializeField]
    private AudioClip gameStart;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        audioSource = GetComponent<AudioSource>();
        PlayClip(gameStart);
    }

    public int GetLevel()
    {
        return level;
    }

    public void GotoLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void GotoNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EnemyDied()
    {
        enemies--;
        if (enemies <= 0)
        {
            Invoke("GotoNextLevel", 2f);
        }
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
