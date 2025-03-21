using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HlavneMenuManazer : MonoBehaviour
{
    public TMP_Text najvyssieSkoreText; //referencia na textové pole kde sa vypisuje High Skore

    void Start()
    {

        int topSkore = PlayerPrefs.GetInt("Najvyššie Skóre", 0);    //z PlayerPrefs získa klúč "Najvyššie Skóre" ak neexistuje vráti 0
        najvyssieSkoreText.text = "Najvyššie Skóre: " + topSkore;   //zobrazenie hodnoty v textovom poli 
    }

    public void HratHru()
    {
        SceneManager.LoadScene("Hra");  //jednoduchá metóda ktorá načíta scénu Hra
    }

    public void UkonciHru()
    {
        Debug.Log("Hra bola ukončená.");    //v Unity editore neukončí hru ale v Buildnutej hre zatvorí okno
        Application.Quit();
    }

    public void ResetujSkore()
    {
        PlayerPrefs.SetInt("Najvyššie Skóre", 0); //resetuje high score na 0 v PlayerPrefs
        PlayerPrefs.Save();

        SkoreManazer.ResetujSkore();

        AktualizujSkoreText();  //zavolá metódu ktorá aktualizuje text v UI aby ho zobrazovalo ako 0
    }

    void AktualizujSkoreText()
    {
        int topSkore = PlayerPrefs.GetInt("Najvyššie Skóre", 0);    //len načita hodnotu a prepíše ju v UI Texte
        najvyssieSkoreText.text = "Najvyššie Skóre: " + topSkore;   //používa sa na okmanžité zobrazenie zresetovanej hodnoty
    }
}