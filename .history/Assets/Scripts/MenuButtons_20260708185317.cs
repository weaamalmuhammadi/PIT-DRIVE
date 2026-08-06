using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public GameObject menuCanvas;

public GameObject gameCamera;
public GameObject carController;
public GameObject menuUI;  
  public GameObject settingsUI; 
    void Start()
    {
    
    }
    public void OpenSettings()
{
    menuUI.SetActive(false);
    settingsUI.SetActive(true);
}

public void BackToMenu()
{
    settingsUI.SetActive(false);
    menuUI.SetActive(true);
}
public void Play()
    {
       
SceneManager.LoadScene("Main Scene");
  
    }
 public void settings()
    {
        
    }
   public void QuitGame()
    {
        
        Application.Quit(); 
    }
    void Update()
    {
        
    }
}
