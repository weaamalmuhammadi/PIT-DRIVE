using UnityEngine;
using UnityEngine.SceneManagement;
public class ready : MonoBehaviour
{public GameObject readyPanel;
public CarController carController;
  public Camera menuCamera;   
    public Camera gameCamera; 
    public void OnReadyClick()
    {
   readyPanel.SetActive(false);
    }
    public void GoToNextScene()
{
    carController.GetComponent<MonoBehaviour>().enabled = true;

}
public void StartGame()
    {
        if (readyPanel != null)
            readyPanel.SetActive(false);

        if (carController != null)
            carController.enabled = true;

        // بدّل الكاميرات
        if (menuCamera != null)
            menuCamera.gameObject.SetActive(false);

        if (gameCamera != null)
            gameCamera.gameObject.SetActive(true);
    }
    }
