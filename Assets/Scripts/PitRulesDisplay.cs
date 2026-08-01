using System.Linq;
using TMPro;
using UnityEngine;

public class PitRulesDisplay : MonoBehaviour
{
    public TMP_Text text;
    public WheelFocusZone[] zones;
    public CarController carController;

    [TextArea]
    public string movingText =
        "<b>PIT STOP GUIDE</b>\nWalk to a wheel and press F to start working on it.";

    [TextArea]
    public string focusedText =
        "<b>WORKING ON WHEEL</b>\nLeft-Click a lug nut to loosen it.\nRight-Click a lug nut to tighten it.\nPress F when done to move to the next wheel.";

    [TextArea]
    public string finishedText =
        "<b>PIT STOP COMPLETE!</b>\nGet back in the car and drive off.";

    [TextArea]
    public string tireWarningText =
        "You need to change the Tires in the PIT Area as fast as possible";

    void Update()
    {
        if (text == null) return;

        if (carController != null && carController.enabled)
        {
            ShowDrivingText();
            return;
        }

        bool allDone = zones != null && zones.Length > 0 && zones.All(z => z.ServiceDone);
        bool isFocused = FocusManager.Instance != null && FocusManager.Instance.IsFocused;

        text.text = allDone ? finishedText : isFocused ? focusedText : movingText;
    }

    void ShowDrivingText()
    {
        string distance = $"<b>Distance driven:</b> {carController.distanceKm:F1} km";

        text.text = carController.TireWarning
            ? $"{distance}\n<color=red>{tireWarningText}</color>"
            : distance;
    }
}
