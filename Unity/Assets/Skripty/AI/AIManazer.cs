using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManazer : MonoBehaviour
{

    [SerializeField]
    AutoManazer autoManazer;

    [SerializeField]
    LayerMask ostatneAutaLayerMaska;

    [SerializeField]
    MeshCollider meshCollider;

    RaycastHit[] raycastsHits = new RaycastHit[1];  //malé pole do ktorého sa ukladajú výsledky z Physics.BoxCastNonAlloc
    bool autoJePred = false;    //bool hodnota ktorá určuje či sa pred AI autom nachádza dalšie auto, ak sa tam nachádza napríklad hráč alebo AI auto tak AI auto stojí

    int jazdaVPruhu = 0;    //tu sa označuje index pruhu v ktorom má AI auto jazdiť.

    WaitForSeconds cakaj = new WaitForSeconds(0.2f);

    private void Awake()
    {   //Awake() sa spúšta len raz v skripte a to počas načítavania inštancie pred tým než začne hra. volá sa pred Start()
        if (CompareTag("Player"))
        {
            Destroy(this);  //ak má objekt tag "Player" tak sa tento skrip zničí aby sa AI riadenie nespúštalo na hráčovom aute
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateMenejCO());    //korutina, ktorá zistuje či je pred vozidlom iné auto, beží periodicky
    }

    // Update is called once per frame
    void Update()
    {
        float rozbehVstup = 1.0f;       //predvolený plyn, auto sa hýbe vpred
        float ovladanieVstup = 0.0f;    //predvolené auto ide len rovno, nezabáča

        if (autoJePred)
        {
            rozbehVstup = -1;   //ak je pred AI autom auto tak brzdí
        }

        float vyzadovanaPoziciaX = Pruhy.PruhyAuta[jazdaVPruhu];    //zistíme požadovanú pozíciu v jazdnom pruhu x súradnica

        float rozdiel = vyzadovanaPoziciaX - transform.position.x;  //rozdeil medzi požadovanou a aktuálnou x pozíciou

        if (Mathf.Abs(rozdiel) > 0.05f)
        {
            ovladanieVstup = 1.0f * rozdiel;    //jemné zatáčanie aby auto ostalo vycentrované v pruhu
        }

        ovladanieVstup = Mathf.Clamp(ovladanieVstup, -1.0f, 1.0f); //metóda Clamp() má tri parametre. ak sa hodnota nezmestí do intervalu Clamp() nám ju oreže. ak sa zmestí tak nám ju len vráti. premenná bude tým pádom priradená len v intervale -1 až 1, aby enboli extrémne hodnoty pri zatáčaní

        autoManazer.NastavVstup(new Vector2(ovladanieVstup, rozbehVstup));
    }

    IEnumerator UpdateMenejCO()
    {    //korutina nám stále pre istotu pozerá či je auto pred AI autom 
        while (true)
        {
            autoJePred = PozriPreAutaVpredu();  //vráti mi bud true alebo false
            yield return cakaj; //yield return nám zabezpečí že sa korutina preruší a pokračovať bude o 0.2 sekundy. spúšta sa v nekonečnej slučke ale šetrí výkon tým že nejde vkuse ale je tam 200milissekundovy interval. v skratke - nespúšta sa každý frame
        }
    }

    bool PozriPreAutaVpredu()
    {
        meshCollider.enabled = false;   //vypneme mesh collider aby sa nejavil ako prekážka sama sebe, potom v Physics.BoxCastNonAlloc() testujeme či je v smere pred AI autom vo vzdialenosti 2jednotiek nejaké iné auto.

        int pocetHitov = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastsHits, Quaternion.identity, 2, ostatneAutaLayerMaska); //non alloc, týmto to len zapisujem do existujúceho poľa, je to rýchlejšie a šetríme garbage collector, zabranujeme lagovaniu v hre

        meshCollider.enabled = true;    //zapne collider ak sa nachádza auto pred ním. čiže pocetHitov > 0, čiže aspon jeden zásah pred ním

        if (pocetHitov > 0)
        {
            return true;    //auto v ceste
        }

        return false;   //volna jazda v ceste neni nič
    }

    private void OnEnable()
    {    //OnEnable() sa spustí, keď sa skript alebo objekt zapne v hierarchií
        autoManazer.NastavMaxRychlost(Random.Range(2, 4));  //nastaví náhodnú rýchosť AI auta, aby všetký auta nešli rovnako, takto dosiahneme zápchy v úsekoch hry, nepôsobí to repetitívne

        jazdaVPruhu = Random.Range(0, Pruhy.PruhyAuta.Length);  //random hodnota 0 - 1 lavý alebo pravý pruh, toto berieme zo skriptu Pruhy.cs
    }
}
