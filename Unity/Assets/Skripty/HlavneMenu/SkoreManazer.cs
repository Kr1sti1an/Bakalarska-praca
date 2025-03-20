using UnityEngine;

public static class SkoreManazer
{
    public static int AktualneSkore { get; private set; } = 0;

    public static void PridajSkore(int mnozstvo)
    {
        AktualneSkore += mnozstvo;
        if (AktualneSkore > ZiskajHighSkore())
        {
            NastavHighSkore(AktualneSkore);
        }
    }

    public static int ZiskajHighSkore()
    {
        return PlayerPrefs.GetInt("Najvyššie Skóre", 0);
    }

    public static void NastavHighSkore(int noveHighSkore)
    {
        PlayerPrefs.SetInt("Najvyššie Skóre", noveHighSkore);
        PlayerPrefs.Save();
    }

    public static void ResetujSkore()
    {
        AktualneSkore = 0;
    }
}