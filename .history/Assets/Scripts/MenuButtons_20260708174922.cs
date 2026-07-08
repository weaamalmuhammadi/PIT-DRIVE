using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public GameObject menuCanvas;
public GameObject menuCamera;
public GameObject gameCamera;
public GameObject carController;
    void Start()
    {
    
    }
public void Play()
    {
       
 
     menuCanvas.SetActive(false);
       menuCamera.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(true);
  
    }
 public void settings()
    {
        
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
