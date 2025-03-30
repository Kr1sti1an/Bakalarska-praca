using UnityEngine;
using UnityEngine.SceneManagement;

public class PauzaMenuManazer : MonoBehaviour
{
    public GameObject pauzaMenu;

    private bool hraPauznuta = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))    //ak sa na klávesnici stlačí kláves P pauzuje sa hra
        {
            if (hraPauznuta)
                PokracujHru();
            else
                PauzujHru();
        }
    }

    public void PauzujHru()
    {
        pauzaMenu.SetActive(true);
        Time.timeScale = 0f; // pozastaví hru
        hraPauznuta = true;
    }

    public void PokracujHru()
    {
        pauzaMenu.SetActive(false);
        Time.timeScale = 1f; // obnovenie normálneho času
        hraPauznuta = false;
    }

    public void DoHlavnehoMenu()    //metóda ktorá vráti do Hlavného menu
    {
        Time.timeScale = 1f; // pred návratom obnov čas
        SceneManager.LoadScene("MenuScena"); // názov scény hlavného menu
    }

    public void UkonciHru()
    {
        Application.Quit();
    }
}