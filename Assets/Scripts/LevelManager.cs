using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager singleton;
    public int TargetLevel
    {
        get
        {
            return targetLevel; 
        }
        set
        {
            targetLevel = value;
        }
    }

    int targetLevel;
    private void Awake()
    {
        singleton = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(targetLevel);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
