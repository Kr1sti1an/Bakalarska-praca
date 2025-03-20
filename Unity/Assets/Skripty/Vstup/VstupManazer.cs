using UnityEngine;
using UnityEngine.SceneManagement;

public class VstupManazer : MonoBehaviour
{
    [SerializeField] private AutoManazer autoManazer;

    [SerializeField] private GameObject pauzaMenu;
    private bool jePauza = false;

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

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!jePauza)
            {
                pauzaMenu.SetActive(true);
                Time.timeScale = 0f;
                jePauza = true;
            }
            else
            {
                pauzaMenu.SetActive(false);
                Time.timeScale = 1f;
                jePauza = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SkoreManazer.ResetujSkore();
            Time.timeScale = 1f;

            Instance = null;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Vector2 vstup = Vector2.zero;

        if (useArduinoInput)
        {
            vstup.x = arduinoSteering;

            if (arduinoForward && arduinoBrake)
                vstup.y = 0f;
            else if (arduinoForward)
                vstup.y = 1f;
            else if (arduinoBrake)
                vstup.y = -1f;
            else
                vstup.y = 0f;
        }
        else
        {
            vstup.x = Input.GetAxis("Horizontal");
            vstup.y = Input.GetAxis("Vertical");
        }

        autoManazer.NastavVstup(vstup);
    }

    public void NastavVstupZArduina(float steering, bool forward, bool brake)
    {
        arduinoSteering = steering;

        const float deadZone = 0.1f;
        if (Mathf.Abs(arduinoSteering) < deadZone)
            arduinoSteering = 0f;

        arduinoForward = forward;
        arduinoBrake = brake;
    }
}