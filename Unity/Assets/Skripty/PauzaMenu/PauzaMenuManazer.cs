using UnityEngine;
using UnityEngine.SceneManagement;

public class PauzaMenuManazer : MonoBehaviour
{
    public GameObject pauzaMenu;    //referencia na Canvas pauza menu

    private bool hraPauznuta = false;   //bool hodnota či je hra práve pozastavená

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

    public void PauzujHru() //metóda na pauzovanie hry
    {
        pauzaMenu.SetActive(true);
        Time.timeScale = 0f; // pozastaví hru
        hraPauznuta = true;
    }

    public void PokracujHru()   //metóda na pokračovanie v hre
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

    public void UkonciHru() //ukončí hru v Buildnutej aplikácií
    {
        Application.Quit();
    }
}