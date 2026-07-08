using UnityEngine;
using UnityEngine.InputSystem;

public class ImpactWrench : MonoBehaviour
{
    public float wrenchRange = 1.5f;
    public LayerMask nutLayer;
    public float wrenchVisualDistance = 0.3f;
    public float crosshairSize = 20f;
    public float crosshairYOffset = 20f;

    Vector2 cursorPos;
    bool wasFocused;
    bool focused;

    void Update()
    {
        focused = FocusManager.Instance != null && FocusManager.Instance.IsFocused;
        Camera activeCam = focused ? FocusManager.Instance.CurrentWheelCamera : null;

        if (focused && !wasFocused)
            cursorPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        wasFocused = focused;

        Transform wrench = activeCam != null ? FindDeep(activeCam.transform, "WrenchModel") : null;
        wrench?.gameObject.SetActive(focused);

        if (!focused || activeCam == null) return;

        if (Mouse.current != null)
        {
            cursorPos += Mouse.current.delta.ReadValue();
            cursorPos.x = Mathf.Clamp(cursorPos.x, 0, Screen.width);
            cursorPos.y = Mathf.Clamp(cursorPos.y, 0, Screen.height);
        }

        Ray aimRay = activeCam.ScreenPointToRay(new Vector3(cursorPos.x, cursorPos.y, 0));
        bool didHit = Physics.Raycast(aimRay, out RaycastHit hit, wrenchRange, nutLayer);

        if (wrench != null)
            wrench.position = didHit ? hit.point : aimRay.origin + aimRay.direction * wrenchVisualDistance;

        if (didHit && Mouse.current != null)
        {
            LugNut nut = hit.collider.GetComponent<LugNut>();
            if (nut != null)
            {
                WheelController wheel = FocusManager.Instance.CurrentWheel;
                if (wheel.state == WheelState.Loosening && Mouse.current.leftButton.isPressed)
                    nut.Loosen(Time.deltaTime);
                else if (wheel.state == WheelState.Tightening && Mouse.current.rightButton.isPressed)
                    nut.Tighten(Time.deltaTime);
            }
        }
    }

    void OnGUI()
    {
        if (!focused) return;
        float x = cursorPos.x;
        float y = Screen.height - cursorPos.y - crosshairYOffset;
        float half = crosshairSize / 2f;
        float gap = crosshairSize * 0.2f;

        GUI.DrawTexture(new Rect(x - half, y - 1f, half - gap, 2f), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(x + gap, y - 1f, half - gap, 2f), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(x - 1f, y - half, 2f, half - gap), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(x - 1f, y + gap, 2f, half - gap), Texture2D.whiteTexture);
    }

    static Transform FindDeep(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform found = FindDeep(child, name);
            if (found != null) return found;
        }
        return null;
    }
}
