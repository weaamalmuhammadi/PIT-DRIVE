using UnityEngine;
using UnityEngine.SceneManagement;
public class ready : MonoBehaviour
{
    public void OnReadyClick()
    {
        SceneManager.LoadScene("اسم_السين_اللي_فيها_اللعبة");
        // أو لو نفس السين: Time.timeScale = 1; gameObject.SetActive(false);
    }
}
