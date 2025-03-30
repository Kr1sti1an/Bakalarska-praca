using UnityEngine;
using TMPro;

public class HernyUIManazer : MonoBehaviour
{
    public TMP_Text najvyssieSkoreText;
    public TMP_Text aktualneSkoreText;
    public TMP_Text casText;

    private float startovaciCas;

    void Start()
    {
        startovaciCas = Time.time;  //uloží sekundy od spustenia hry do premennej
    }

    void Update()
    {
        int najvyssieSkore = PlayerPrefs.GetInt("Najvyššie Skóre", 0);  //v PlayerPrefs hladáme kľuč "Najvyššie Skóre", ak neexistuje vráti 0
        najvyssieSkoreText.text = "Najvyššie Skóre: " + najvyssieSkore;

        aktualneSkoreText.text = "Aktuálne Skóre: " + SkoreManazer.AktualneSkore;   //v skripte SkoreManazer je premenlivá statická premenná ktorá uchováva skóre

        float ubehlo = Time.time - startovaciCas;
        int minuty = Mathf.FloorToInt(ubehlo / 60f);
        int sekundy = Mathf.FloorToInt(ubehlo % 60f);
        casText.text = string.Format(" Čas: {0:00}:{1:00}", minuty, sekundy);   //základný výpočet času aby sa to zobrazovalo vo formáte MM:SS
    }
}