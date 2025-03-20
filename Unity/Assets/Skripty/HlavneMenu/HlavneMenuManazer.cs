using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HlavneMenuManazer : MonoBehaviour
{
    public TMP_Text najvyssieSkoreText;

    void Start()
    {

        int topSkore = PlayerPrefs.GetInt("Najvyššie Skóre", 0);
        najvyssieSkoreText.text = "Najvyššie Skóre: " + topSkore;
    }

    public void HratHru()
    {
        SceneManager.LoadScene("Hra");
    }

    public void UkonciHru()
    {
        Debug.Log("Hra bola ukončená.");
        Application.Quit();
    }

    public void ResetujSkore()
    {
        PlayerPrefs.SetInt("Najvyššie Skóre", 0);
        PlayerPrefs.Save();

        SkoreManazer.ResetujSkore();

        AktualizujSkoreText();
    }

    void AktualizujSkoreText()
    {
        int topSkore = PlayerPrefs.GetInt("Najvyššie Skóre", 0);
        najvyssieSkoreText.text = "Najvyššie Skóre: " + topSkore;
    }
}