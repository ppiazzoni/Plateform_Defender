using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject gameOverUI;
    public Canvas BGCanvas;

    void Start()
    {
        BGCanvas.worldCamera  = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameOver()
    {
        gameOverUI.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
   
    public void StartGame()
    {
        SceneManager.LoadScene("PlatformDefender");
    }
   
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
      Application.Quit();
    }
}
