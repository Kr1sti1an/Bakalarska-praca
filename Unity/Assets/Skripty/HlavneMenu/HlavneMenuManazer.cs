using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HlavneMenuManazer : MonoBehaviour
{
    public TMP_Text najvyssieSkoreText; // Odkaz na High Score text

    void Start()
    {

        int topSkore = PlayerPrefs.GetInt("Najvyssie Skore", 0);
        najvyssieSkoreText.text = "Najvyssie Skore " + topSkore;
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
}