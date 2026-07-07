using UnityEngine;

public class MenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
public void Play()
    {

        SceneManager.LoadScene("Car Game"); 
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
