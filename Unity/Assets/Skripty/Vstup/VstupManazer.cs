using UnityEngine;
using UnityEngine.SceneManagement;

public class VstupManazer : MonoBehaviour
{
    [SerializeField] private AutoManazer autoManazer;

    public static VstupManazer Instance;

    private float arduinoSteering = 0f;
    private bool arduinoForward = false;
    private bool arduinoBrake = false;

    public bool useArduinoInput = true;

    private void Awake()
    {
        if (!CompareTag("Player"))
        {
            Destroy(this);
            return;
        }

        // Singleton pre jednoduchý prístup z iných skriptov
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        Vector2 vstup = Vector2.zero;

        if (useArduinoInput)
        {
            vstup.x = arduinoSteering;

            if (arduinoForward && arduinoBrake)
                vstup.y = 0f; // Obe tlačidlá stlačené - auto stojí
            else if (arduinoForward)
                vstup.y = 1f;
            else if (arduinoBrake)
                vstup.y = -1f;
            else
                vstup.y = 0f;
        }
        else
        {
            // Použitie vstupov z klávesnice
            vstup.x = Input.GetAxis("Horizontal");
            vstup.y = Input.GetAxis("Vertical");
        }

        autoManazer.NastavVstup(vstup);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SkoreManazer.ResetujSkore();
            Time.timeScale = 1f;
            Instance = null;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void NastavVstupZArduina(float steering, bool forward, bool brake)
    {
        arduinoSteering = steering;

        // Pridanie mŕtvej zóny pre volant
        const float deadZone = 0.1f; // 10% mŕtva zóna
        if (Mathf.Abs(arduinoSteering) < deadZone)
            arduinoSteering = 0f;

        arduinoForward = forward;
        arduinoBrake = brake;
    }
}