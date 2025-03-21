using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManazer : MonoBehaviour
{

    public void ZnovaHrat()
    {
        SkoreManazer.ResetujSkore();    //vynuluje aktuálne skore
        SceneManager.LoadScene("Hra");  //načíta hlavnú scénu
    }

    public void DoHlavnehoMenu()
    {
        SceneManager.LoadScene("MenuScena");    //prepne do menu
    }

    public void UkonciHru()
    {
        Debug.Log("Hra bola ukončená.");    //ukončí hru (len v Buildnutej aplikácií)
        Application.Quit();
    }
}