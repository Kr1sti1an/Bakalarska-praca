using UnityEngine;
using UnityEngine.SceneManagement;

public class PauzaMenuManazer : MonoBehaviour
{
    // Referencia na GameObject pauzového menu (priradíš ho v Inspector)
    public GameObject pauzaMenu;

    // Indikátor, či je hra práve pozastavená
    private bool hraPauznuta = false;

    void Update()
    {
        // Ak stlačíme kláves P, prepni pauzu
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (hraPauznuta)
                PokracujHru();
            else
                PauzujHru();
        }
    }

    // Metóda na pauzovanie hry
    public void PauzujHru()
    {
        pauzaMenu.SetActive(true);
        Time.timeScale = 0f; // pozastaví hru
        hraPauznuta = true;
    }

    // Metóda na pokračovanie v hre
    public void PokracujHru()
    {
        pauzaMenu.SetActive(false);
        Time.timeScale = 1f; // obnovenie normálneho času
        hraPauznuta = false;
    }

    // Metóda volaná tlačidlom "Hlavné menu"
    public void DoHlavnehoMenu()
    {
        Time.timeScale = 1f; // pred návratom obnov čas
        SceneManager.LoadScene("MenuScena"); // názov scény hlavného menu
    }

    // Metóda volaná tlačidlom "Ukončiť hru"
    public void UkonciHru()
    {
        Application.Quit();
    }
}