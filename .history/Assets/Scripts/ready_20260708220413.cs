using UnityEngine;
public class ready : MonoBehaviour
{public GameObject readyPanel;
    public void OnReadyClick()
    {
   readyPanel.SetActive(false);
    }
}
