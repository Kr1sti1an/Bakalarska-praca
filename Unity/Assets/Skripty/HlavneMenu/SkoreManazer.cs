using UnityEngine;

public static class SkoreManazer //statická čiže nemôžeme vytvoriť jej inštanciu new SkoreManager(), využívame len jej public metódy
{
    public static int AktualneSkore { get; private set; } = 0;  //hodnota sa nastavuje len v tejto triede ale čitať ju môže hocikto kto pristupuje k tejto triede

    public static void PridajSkore(int mnozstvo)    //zvačší aktuálne skóre o jedna, ked sa prejde po minci
    {
        AktualneSkore += mnozstvo;
        if (AktualneSkore > ZiskajHighSkore())
        {
            NastavHighSkore(AktualneSkore);
        }
    }

    public static int ZiskajHighSkore()
    {
        return PlayerPrefs.GetInt("Najvyššie Skóre", 0);    //vráti int uložený v PlayerPrefs pri klúči "Najvyššie Skóre"
    }

    public static void NastavHighSkore(int noveHighSkore)
    {
        PlayerPrefs.SetInt("Najvyššie Skóre", noveHighSkore);   //uloží hodnotu noveHighSkore do PlayerPrefs
        PlayerPrefs.Save(); //toto zabezpečí že sa na disk zapíše okamžite, neprídeme o skore ani po reštarte hry alebo náhlom vypnutí
    }

    public static void ResetujSkore()
    {
        AktualneSkore = 0;  //resetuje skore na 0
    }
}