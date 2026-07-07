using UnityEngine;
using UnityEngine.InputSystem;

public class ImpactWrench : MonoBehaviour
{
    public float wrenchRange = 1.5f;
    public LayerMask nutLayer;
    public float wrenchVisualDistance = 0.3f;
    public float crosshairSize = 6f;

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
        bool aimHit = Physics.Raycast(aimRay, out RaycastHit aimHitInfo, wrenchRange, nutLayer);

        if (wrench != null)
            wrench.position = aimHit ? aimHitInfo.point : aimRay.origin + aimRay.direction * wrenchVisualDistance;

        Transform muzzle = wrench != null ? FindDeep(wrench, "SM_Drill_01_Spindel_B") : null;
        Ray shootRay = muzzle != null ? new Ray(muzzle.position, aimRay.direction) : aimRay;
        bool didHit = Physics.Raycast(shootRay, out RaycastHit hit, wrenchRange, nutLayer);

        if (Mouse.current != null && Mouse.current.leftButton.isPressed && didHit)
        {
            LugNut nut = hit.collider.GetComponent<LugNut>();
            if (nut != null)
            {
                WheelController wheel = FocusManager.Instance.CurrentWheel;
                if (wheel.state == WheelState.Loosening) nut.Loosen(Time.deltaTime);
                else if (wheel.state == WheelState.Tightening) nut.Tighten(Time.deltaTime);
            }
        }
    }

    void OnGUI()
    {
        if (!focused) return;
        float half = crosshairSize / 2f;
        GUI.DrawTexture(new Rect(cursorPos.x - half, Screen.height - cursorPos.y - half, crosshairSize, crosshairSize), Texture2D.whiteTexture);
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
