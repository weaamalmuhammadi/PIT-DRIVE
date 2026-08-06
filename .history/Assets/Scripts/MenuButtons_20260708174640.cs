using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButtons : MonoBehaviour
{
    public GameObject menuCanvas;
public GameObject menuCamera;
public GameObject gameCamera;
    void Start()
    {
    
    }
public void Play()
    {
       
 
     menuCanvas.SetActive(false);
       menuCamera.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(true);
        carController.enabled = true;
    }
   public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
            menuCamera.SetActive(true);
          menuCanvas.SetActive(true);
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
