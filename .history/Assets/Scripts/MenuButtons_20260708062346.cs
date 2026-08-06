using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
public void Play()
    {
      
 
        menuCanvas.SetActive(false);
        menuCamera.SetActive(false);
        gameCamera.SetActive(true);
        carController.enabled = true;
    }
    public void OpenSettings()
    {
        Debug.Log("Settings opened!");
       
    }
   public void QuitGame()
    {
        Debug.Log("Quit game!");
        Application.Quit(); 
    }
    void Update()
    {
        
    }
}
