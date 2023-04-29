using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int level = 0;
    private int enemies = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            level = SceneManager.GetActiveScene().buildIndex;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public int GetLevel()
    {
        return level;
    }

    public void GotoLevel(int level)
    {
        this.level = level;
        SceneManager.LoadScene(level);
    }

    public void GotoNextLevel()
    {
        GotoLevel(level + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(level);
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
}
