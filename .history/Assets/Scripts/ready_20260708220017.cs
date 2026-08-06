using UnityEngine;
using UnityEngine.SceneManagement;
public class ready : MonoBehaviour
{
    public void OnReadyClick()
    {
   SceneManager.LoadScene("Main Scene");
    }
}
