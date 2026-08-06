using UnityEngine;
public class CameraSwitcher : MonoBehaviour
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
        menuCamera.gameObject.SetActive(true);
        settingsCamera.gameObject.SetActive(false);
    }

    public void ShowSettingsCamera()
    {
        menuCamera.gameObject.SetActive(false);
        settingsCamera.gameObject.SetActive(true);
    }
     public void cameraGame()
    {
        menuCamera.gameObject.SetActive(false);
        gameCamera.gameObject.SetActive(true);
    }
}

