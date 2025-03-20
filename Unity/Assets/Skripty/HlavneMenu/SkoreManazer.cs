using UnityEngine;

public static class SkoreManazer
{
    public static int AktualneSkore { get; private set; } = 0;

    public static void PridajSkore(int mnozstvo)
    {
        AktualneSkore += mnozstvo;
        Debug.Log("Pridané skóre: " + mnozstvo + ", aktuálne skóre: " + AktualneSkore);
        if (AktualneSkore > ZiskajHighSkore())
        {
            NastavHighSkore(AktualneSkore);
        }
    }

    public static int ZiskajHighSkore()
    {
        return PlayerPrefs.GetInt("Najvyššie Skore", 0);
    }

    public static void NastavHighSkore(int noveHighSkore)
    {
        PlayerPrefs.SetInt("Najvyššie Skore", noveHighSkore);
        PlayerPrefs.Save();
    }

    public static void ResetujSkore()
    {
        AktualneSkore = 0;
    }
}