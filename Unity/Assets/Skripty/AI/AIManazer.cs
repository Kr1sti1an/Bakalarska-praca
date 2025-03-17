using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManazer : MonoBehaviour
{

    [SerializeField]
    AutoManazer autoManazer;

    //Detekcia kolizie
    [SerializeField]
    LayerMask ostatneAutaLayerMaska;

    [SerializeField]
    MeshCollider meshCollider;

    RaycastHit[] raycastsHits = new RaycastHit[1];
    bool autoJePred = false;

    //Pruhy
    int jazdaVPruhu = 0;

    //Casovanie
    WaitForSeconds cakaj = new WaitForSeconds(0.2f);

    private void Awake(){
        if (CompareTag("Player")){
            Destroy(this);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateMenejCO());
    }

    // Update is called once per frame
    void Update()
    {
        float rozbehVstup = 1.0f;
        float ovladanieVstup = 0.0f;

        if (autoJePred){
            rozbehVstup = -1;
        }

        float vyzadovanaPoziciaX = Pruhy.PruhyAuta[jazdaVPruhu];

        float rozdiel = vyzadovanaPoziciaX - transform.position.x;

        if(Mathf.Abs(rozdiel) > 0.05f){
            ovladanieVstup = 1.0f * rozdiel;
        }

        ovladanieVstup = Mathf.Clamp(ovladanieVstup, -1.0f, 1.0f);

        autoManazer.NastavVstup(new Vector2(ovladanieVstup, rozbehVstup));
    }

    IEnumerator UpdateMenejCO(){
        while (true){
            autoJePred = PozriPreAutaVpredu();
            yield return cakaj;
        }
    }

    bool PozriPreAutaVpredu(){
        meshCollider.enabled = false;

        int pocetHitov = Physics.BoxCastNonAlloc(transform.position, Vector3.one * 0.25f, transform.forward, raycastsHits, Quaternion.identity, 2, ostatneAutaLayerMaska);

        meshCollider.enabled = true;

        if(pocetHitov > 0){
            return true;
        }

        return false;
    }

    //Eventy
    private void OnEnable(){
        //Nastavenie nahodnej rychlosti aut
        autoManazer.NastavMaxRychlost(Random.Range(2, 4));

        //Nastavenie nahodneho pruhu v ktorom auta pojdu
        jazdaVPruhu = Random.Range(0, Pruhy.PruhyAuta.Length);
    }
}
