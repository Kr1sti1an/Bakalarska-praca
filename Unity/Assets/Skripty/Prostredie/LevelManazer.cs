using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManazer : MonoBehaviour
{
    [SerializeField]
    GameObject[] sekciePrefaby; //pole prefabov, čiže rôznych sekcií cesty

    GameObject[] sekciePole = new GameObject[20];   //pole 20 instancií sekcií, slúži ako pool(zásobník), z ktorého vyberáme volné sekcie

    GameObject[] sekcie = new GameObject[10];   //pole 10 sekcií ktoré sú v danom momente aktívne v scéne

    Transform hracAutoTransform;    //poloha hráča aby sme vedeli zistiť kedy je sekcia daleko za hráčom

    WaitForSeconds cakaj100ms = new WaitForSeconds(0.1f);   //korutina bude každých 0.1 sekundy kontrolovať pozície sekcií

    const float dlzkaSekcie = 26;   //dl'žka jednej sekcie po osi Z, pri posúvaní sekcie dopredu sa pripočítavajú násobky 26

    // Start is called before the first frame update
    void Start()
    {
        hracAutoTransform = GameObject.FindGameObjectWithTag("Player").transform;   //hračova poloha

        int prefabIndex = 0;

        //Vytvorenie pola pre nekonečné sekcie
        for (int i = 0; i < sekciePole.Length; i++)
        {
            sekciePole[i] = Instantiate(sekciePrefaby[prefabIndex]); // v cykle sa inštancujú všetky možne prefaby, ak dôjdeme na koniec sekciePrefaby, začína sa od začiatku pomocou prefabIndex
            sekciePole[i].SetActive(false); //sekcie sú vypnuté kým ich nepotrebujeme

            prefabIndex++;

            if (prefabIndex > sekciePrefaby.Length - 1)
            {
                prefabIndex = 0;    //začína sa od začiatku
            }
        }

        //Pridanie prvych sekcií na cestu
        for (int i = 0; i < sekcie.Length; i++)
        {    //cyklus 0-9 vracia nám neaktívne sekcie z poolu
            //Ziskajme nahodnu sekciu 
            GameObject nahodnaSekcia = ZiskajNahoduSekciu();

            //Presunieme to na poziciu a nastavime na active
            nahodnaSekcia.transform.position = new Vector3(sekciePole[i].transform.position.x, -10, i * dlzkaSekcie);   //tu je zabezpečené že sa sekcie kladú jedna za druhou a sú umiestenné na scénu, vytvárajú cestu
            nahodnaSekcia.SetActive(true);  //aktivujeme aby sa zobrazila na scéne

            //Nastavenie sekcie v poli
            sekcie[i] = nahodnaSekcia;  //ukladáme ju do poľa sekcie
        }

        StartCoroutine(UpdateMenejCO());
    }

    IEnumerator UpdateMenejCO()
    {
        while (true)
        {
            UpdateSekciaPozicie();
            yield return cakaj100ms;    //šetrí sa výkon beží neustále ale nie každý frame
        }
    }

    void UpdateSekciaPozicie()
    {
        for (int i = 0; i < sekcie.Length; i++)
        {    //cyklus cez aktuálne aktívne sekcie
            if (sekcie[i].transform.position.z - hracAutoTransform.position.z < -dlzkaSekcie)
            {   //ak je sekcia za hráčom o celú dĺžku ju vypne
                Vector3 poslednaPoziciaSekcie = sekcie[i].transform.position;
                sekcie[i].SetActive(false);

                sekcie[i] = ZiskajNahoduSekciu();   //získame novú sekciu z poolu

                sekcie[i].transform.position = new Vector3(poslednaPoziciaSekcie.x, -10, poslednaPoziciaSekcie.z + dlzkaSekcie * sekcie.Length);    //posunieme ju op 260 jednotiek oproti jej starej pozícií, a takto vzniká ilúzia nekonečnej trate
                sekcie[i].SetActive(true);  //aktivujeme sekciu do ktorej hráč v budúcnosti príde
            }
        }
    }

    GameObject ZiskajNahoduSekciu()
    {
        //Vyberie nahodny index 
        int nahodnyIndex = Random.Range(0, sekciePole.Length);  //random index v rozashu 0-19

        bool najdenaNovaSekcia = false;

        while (!najdenaNovaSekcia)
        {
            if (!sekciePole[nahodnyIndex].activeInHierarchy)
            {    //ak je neaktivna sekcia ju vyberáme a končí sa cyklus
                najdenaNovaSekcia = true;
            }
            else
            {
                nahodnyIndex++; //ak je bool na true zvyšuje sa index(musí sa vybrať neaktívna sekcia)

                if (nahodnyIndex > sekciePole.Length - 1)
                {
                    nahodnyIndex = 0;
                }
            }
        }

        return sekciePole[nahodnyIndex];    //z poolu sa vyberá náhodný GameObject sekcie ktorá sa použije v UpdateSekciapozicie() alebo pri prvotnom rozkladaní cesty
    }

}
