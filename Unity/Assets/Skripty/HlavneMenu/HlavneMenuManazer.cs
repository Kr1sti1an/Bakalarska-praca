using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HlavneMenuManazer : MonoBehaviour
{
    public TMP_Text najvyssieSkoreText;

    void Start()
    {

        int topSkore = PlayerPrefs.GetInt("Najvyššie Skore", 0);
        najvyssieSkoreText.text = "Najvyššie Skore " + topSkore;
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