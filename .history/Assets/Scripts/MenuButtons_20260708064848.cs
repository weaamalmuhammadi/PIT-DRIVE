using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public GameObject menuCanvas;
public GameObject menuCamera;
public GameObject gameCamera;
public MonoBehaviour carController;
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
