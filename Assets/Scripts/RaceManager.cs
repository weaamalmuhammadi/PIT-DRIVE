using System.Collections;
using UnityEngine;
using TMPro;

public class RaceManager : MonoBehaviour
{
    public enum RaceState { AutoDriveIn, Ready, Countdown, Racing, Finished }

    [Header("السيارة")]
    public Rigidbody carRigidbody;          // Rigidbody الخاص بالسيارة
    public MonoBehaviour carInputScript;    // سكربت تحكم اللاعب بالسيارة (يوقف/يشتغل تلقائي)

    [Header("واجهة المستخدم UI")]
    public TextMeshProUGUI readyText;       // نص Ready? و 3 2 1
    public TextMeshProUGUI lapText;         // Lap 1/2
    public TextMeshProUGUI timerText;       // الوقت الحالي
    public GameObject resultPanel;          // بانل النتيجة النهائية
    public TextMeshProUGUI resultText;      // نص النتيجة والرقم القياسي

    [Header("إعدادات السباق")]
    public int totalLaps = 2;               // عدد اللفات المطلوبة للفوز
    public float autoDriveSpeed = 5f;       // سرعة الحركة التلقائية بالبداية
    public float autoDriveDuration = 1.5f;  // مدة الحركة التلقائية قبل ما توصل الخط
    public float lapCooldown = 3f;          // مهلة بين كل احتساب لفة (تمنع التكرار)

    private RaceState state;
    private int currentLap = 0;
    private float raceTimer = 0f;
    private bool timerRunning = false;
    private float bestTime = -1f;
    private bool canCountLap = true;

    void Start()
    {
        bestTime = PlayerPrefs.GetFloat("BestLapTime", -1f);

        if (carInputScript != null) carInputScript.enabled = false; // نوقف تحكم اللاعب بالبداية
        if (resultPanel != null) resultPanel.SetActive(false);
        if (readyText != null) readyText.gameObject.SetActive(false);

        StartCoroutine(AutoDriveIn());
    }

    IEnumerator AutoDriveIn()
    {
        state = RaceState.AutoDriveIn;
        float t = 0f;
        while (t < autoDriveDuration)
        {
            carRigidbody.MovePosition(carRigidbody.position + carRigidbody.transform.forward * autoDriveSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
        carRigidbody.linearVelocity = Vector3.zero; // إذا يعطيك خطأ استخدم carRigidbody.velocity حسب نسخة يونتي
        StartCoroutine(ReadyAndCountdown());
    }

    IEnumerator ReadyAndCountdown()
    {
        state = RaceState.Ready;
        readyText.gameObject.SetActive(true);
        readyText.text = "Ready?";
        yield return new WaitForSeconds(1.2f);

        state = RaceState.Countdown;
        string[] counts = { "3", "2", "1", "GO!" };
        foreach (string c in counts)
        {
            readyText.text = c;
            yield return new WaitForSeconds(0.8f);
        }
        readyText.gameObject.SetActive(false);

        BeginRace();
    }

    void BeginRace()
    {
        state = RaceState.Racing;
        currentLap = 0;
        raceTimer = 0f;
        timerRunning = true;
        UpdateLapUI();
        if (carInputScript != null) carInputScript.enabled = true; // نرجع التحكم للاعب
    }

    void Update()
    {
        if (timerRunning)
        {
            raceTimer += Time.deltaTime;
            if (timerText != null)
                timerText.text = FormatTime(raceTimer);
        }
    }

    // تستدعى من سكربت FinishLineTrigger لما السيارة تقطع الخط
    public void OnCarCrossedFinish()
    {
        if (state != RaceState.Racing) return;
        if (!canCountLap) return;

        currentLap++;
        UpdateLapUI();
        canCountLap = false;
        StartCoroutine(LapCooldown());

        if (currentLap >= totalLaps)
        {
            FinishRace();
        }
    }

    IEnumerator LapCooldown()
    {
        yield return new WaitForSeconds(lapCooldown);
        canCountLap = true;
    }

    void UpdateLapUI()
    {
        if (lapText != null)
            lapText.text = $"Lap {Mathf.Min(currentLap, totalLaps)}/{totalLaps}";
    }

    void FinishRace()
    {
        state = RaceState.Finished;
        timerRunning = false;
        if (carInputScript != null) carInputScript.enabled = false;

        bool isNewRecord = bestTime < 0 || raceTimer < bestTime;
        if (isNewRecord)
        {
            bestTime = raceTimer;
            PlayerPrefs.SetFloat("BestLapTime", bestTime);
            PlayerPrefs.Save();
        }

        if (resultPanel != null) resultPanel.SetActive(true);
        if (resultText != null)
        {
            resultText.text = isNewRecord
                ? $"رقم قياسي جديد!\n{FormatTime(raceTimer)}"
                : $"وقتك: {FormatTime(raceTimer)}\nالأفضل: {FormatTime(bestTime)}";
        }
    }

    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        int seconds = Mathf.FloorToInt(t % 60f);
        int millis = Mathf.FloorToInt((t * 1000f) % 1000f);
        return $"{minutes:00}:{seconds:00}.{millis:000}";
    }
}

