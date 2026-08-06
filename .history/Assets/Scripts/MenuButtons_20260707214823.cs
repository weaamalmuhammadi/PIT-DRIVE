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

        SceneManager.LoadScene("Main Scene");
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
