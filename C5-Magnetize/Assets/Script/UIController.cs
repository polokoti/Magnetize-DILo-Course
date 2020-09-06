using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject resumeBtn;
    public GameObject lvlClearTxt;
    public GameObject nextBtn;
    public GameObject exitBtn;
    private Scene currActiveScene;
    //private Scene nextLevelScene;
    private int currentSceneIndex;

    

    // Start is called before the first frame update
    void Start()
    {
        currActiveScene = SceneManager.GetActiveScene();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Scene sceneLoaded = SceneManager.GetActiveScene();
        //nextLevelScene = SceneManager.LoadScene;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        nextBtn.SetActive(false);
        exitBtn.SetActive(true);
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void restartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currActiveScene.name);
    }

    public void nextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentSceneIndex +1);
    }

    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Exit");
    }

    public void endGame()
    {
        pausePanel.SetActive(true);
        resumeBtn.SetActive(false);
        lvlClearTxt.SetActive(true);
        nextBtn.SetActive(true);
        exitBtn.SetActive(true);
    }
}
