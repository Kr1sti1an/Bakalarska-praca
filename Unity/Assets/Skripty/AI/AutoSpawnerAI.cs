using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnerAI : MonoBehaviour
{

    [SerializeField]
    GameObject[] autoAIPrefaby; //pole gameobject prefabov, napríklad viac modelov AI áut

    GameObject[] autoAIPole = new GameObject[20]; //do tohoto pola sa ukladá inštancia až 20 AI áut

    Transform hracAutoTransform;    //podla polohy hráča sa spawnuju AI auta

    float casOdPoslednehoSpawnu = 0;    //zabezpečuje aby sa AI auta nespawnovali príliš často
    WaitForSeconds cakaj = new WaitForSeconds(0.5f); // v korutine aby bežala každých 0.5 sekundy

    [SerializeField]
    LayerMask ostatneAutaLayerMaska;    // pomocou tejto layer masky sa overuje či je priestor volný na spawn AI auta

    Collider[] kontrolaPredbehnutiaCollider = new Collider[1];  //pomocné pole

    // Start is called before the first frame update
    void Start()
    {

        hracAutoTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        for (int i = 0; i < autoAIPole.Length; i++) // 0 - 19, hned sa vytvorí inštanciami naplnené pole ai prefabov, ktoré sa hneď nastavia na false, predpripravíme pole
        {
            autoAIPole[i] = Instantiate(autoAIPrefaby[prefabIndex]);
            autoAIPole[i].SetActive(false);

            prefabIndex++;

            if (prefabIndex > autoAIPrefaby.Length - 1) //Loop cez prefab index ak sa nám minú prefaby
            {
                prefabIndex = 0;
            }
        }

        StartCoroutine(UpdateMenejCO());
    }

    IEnumerator UpdateMenejCO()
    {
        while (true)
        {
            CistenieAutZaKamerou(); //deaktivuje autá ktoreé sú príliš ďaleko za kamerou
            SpawnNovehoauta();  //aktivuje nové auto ak je splnený priestorový aj časový limit
            yield return cakaj; //yield return zabezpečí že korutina počka 0.5 sekundy a potom vykonáva akcie znova, zabezpečuje plynulejší chod hry tým že nejde s každým framom ale je tam pauza.
        }
    }

    void SpawnNovehoauta()
    {
        if (Time.time - casOdPoslednehoSpawnu < 2)  //ak neuplynuli aspoň 2 sekundy od posledného spawnu tak nespawnujeme
        {
            return;
        }

        GameObject autoNaSpawn = null;

        foreach (GameObject aiAuto in autoAIPole)
        {
            if (aiAuto.activeInHierarchy)   //preskočíme aktívne autá
            {
                continue;
            }

            autoNaSpawn = aiAuto;   //uložíme do autoNaSpawn
            break;
        }

        if (autoNaSpawn == null)    //ak neni žiadne auto na spawn čakáme kým sa neuvolní v korutine sa znova volá táto funkcia
        {
            return;
        }

        Vector3 spawnPozicia = new Vector3(0, 0, hracAutoTransform.transform.position.z + 100); //pozícia kde sa spawne nové auto, čiže 100jednotiek pred hráčom na osi Z

        if (Physics.OverlapBoxNonAlloc(spawnPozicia, Vector3.one * 2, kontrolaPredbehnutiaCollider, Quaternion.identity, ostatneAutaLayerMaska) > 0)    //predchádzame koliízií pri spawne AI, ak tam už je iné auto tak sa nespawnuje
        {
            return;
        }

        autoNaSpawn.transform.position = spawnPozicia;
        autoNaSpawn.SetActive(true);    //zapína auto v scéne

        casOdPoslednehoSpawnu = Time.time;  //zapamätáva si čas od najnovšieho spawnu auta
    }

    void CistenieAutZaKamerou()
    {
        foreach (GameObject aiAuto in autoAIPole)   //foreach cyklus, prechádzame všetký auta v poli
        {
            if (!aiAuto.activeInHierarchy)  //neaktivne preskočíme
            {
                continue;
            }

            if (aiAuto.transform.position.z - hracAutoTransform.position.z > 200)   //ak je daleko pred hráčom deaktivuje sa
            {
                aiAuto.SetActive(false);
            }

            if (aiAuto.transform.position.z - hracAutoTransform.position.z < -50)   //ak je daleko za hráčom tiež sa deaktivuje
            {
                aiAuto.SetActive(false);
            }
        }
    }
}
