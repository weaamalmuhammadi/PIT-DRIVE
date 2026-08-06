using UnityEngine;
public class Camersscript : MonoBehaviour
{
    public Camera menuCamera;
    public Camera settingsCamera;
 public Camera gameCamera;
    void Start()
    {
        ShowMenuCamera();
    }

    public void ShowMenuCamera()
    {
          gameCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);
        settingsCamera.gameObject.SetActive(false);
    }

    public void ShowSettingsCamera()
    {
         gameCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(false);
        settingsCamera.gameObject.SetActive(true);
    }
     public void cameraGame()
    {
          settingsCamera.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(true);
    }
}

