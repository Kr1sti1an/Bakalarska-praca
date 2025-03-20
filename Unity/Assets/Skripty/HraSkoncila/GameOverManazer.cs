using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManazer : MonoBehaviour
{

    public void ZnovaHrat()
    {
        SkoreManazer.ResetujSkore();
        SceneManager.LoadScene("Hra");
    }

    public void DoHlavnehoMenu()
    {
        SceneManager.LoadScene("MenuScena");
    }

    public void UkonciHru()
    {
        Debug.Log("Hra bola ukončená.");
        Application.Quit();
    }
}