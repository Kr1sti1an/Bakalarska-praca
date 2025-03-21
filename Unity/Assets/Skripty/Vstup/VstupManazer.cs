using UnityEngine;
using UnityEngine.SceneManagement;

public class VstupManazer : MonoBehaviour
{
    [SerializeField] private AutoManazer autoManazer; //referencia na AutoManazer ktorý zabezpečuje fyziku a pohyb auta

    [SerializeField] private GameObject pauzaMenu;  //canvas pre pauza menu
    private bool jePauza = false;   //uchováva stav či je hra pauznutá

    public static VstupManazer Instance;    // statická referencia na seba samého(singleton) aby sa z iných skriotov dalo volať napríklad VstupMManazer.Instance.xx()

    // arduino premenné, ukladajú dáta prijaté z Arduina
    private float arduinoSteering = 0f;
    private bool arduinoForward = false;
    private bool arduinoBrake = false;

    public bool useArduinoInput = true; //ak je true skript používa dáta z arduina, inak berie klasický vstup z klávesnice

    private void Awake()    //Awake() sa spúšta len raz a pred Start()
    {
        if (!CompareTag("Player"))  //zabezpečíme že skript bude bežať iba na objekte s tagom Player 
        {
            Destroy(this);
            return;
        }

        if (Instance == null)
            Instance = this;    //singleton, aby v scéne bola vždy len jedna kópia
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))    //pri stlačení P otvorí pause menu
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

        if (Input.GetKeyDown(KeyCode.R))    //pri stlačení R reštartuje scénu (hru) aj so skore
        {
            SkoreManazer.ResetujSkore();
            Time.timeScale = 1f;

            Instance = null;

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Vector2 vstup = Vector2.zero;

        if (useArduinoInput)    //tu sa načítavajú vstupy z arduina
        {
            vstup.x = arduinoSteering;  //zatáčanie auto po osi X na základe hodnôt z potenciometra

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

    public void NastavVstupZArduina(float steering, bool forward, bool brake)   //toto je zo skriptu ArduinoInputManager
    {
        arduinoSteering = steering; //uloží hodnotu zatáčania

        const float deadZone = 0.1f;
        if (Mathf.Abs(arduinoSteering) < deadZone)  //malá mŕtva zóna aby nulové uhly neprekmitávali
            arduinoSteering = 0f;

        arduinoForward = forward;   //stav tlačididla pre plyn
        arduinoBrake = brake;   //stav tlačididla pre brzdu
    }
}