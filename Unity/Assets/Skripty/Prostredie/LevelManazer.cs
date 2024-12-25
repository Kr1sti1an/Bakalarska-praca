using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManazer : MonoBehaviour
{
    [SerializeField]
    GameObject[] sekciePrefaby;

    GameObject[] sekciePole = new GameObject[20];

    GameObject[] sekcie = new GameObject[10];

    Transform hracAutoTransform;

    WaitForSeconds cakaj100ms = new WaitForSeconds(0.1f);

    const float dlzkaSekcie = 26;

    // Start is called before the first frame update
    void Start()
    {
        hracAutoTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        //Vytvorenie pola pre nekonečné sekcie
        for(int i = 0; i < sekciePole.Length; i++){
            sekciePole[i] = Instantiate(sekciePrefaby[prefabIndex]);
            sekciePole[i].SetActive(false);

            prefabIndex++;

            if(prefabIndex > sekciePrefaby.Length - 1){
                prefabIndex = 0;
            }
        }

        //Pridanie prvych sekcií na cestu
        for (int i = 0; i < sekcie.Length; i++){
            //Ziskajme nahodnu sekciu 
            GameObject nahodnaSekcia = ZiskajNahoduSekciu();

            //Presunieme to na poziciu a nastavime na active
            nahodnaSekcia.transform.position = new Vector3(sekciePole[i].transform.position.x, -10, i * dlzkaSekcie);
            nahodnaSekcia.SetActive(true);

            //Nastavenie sekcie v poli
            sekcie[i] = nahodnaSekcia;
        }

        StartCoroutine(UpdateMenejCO());
    }

    IEnumerator UpdateMenejCO(){
        while(true){
            UpdateSekciaPozicie();
            yield return cakaj100ms;
        }
    }

    void UpdateSekciaPozicie(){
        for (int i = 0; i < sekcie.Length; i++){
            //Pozrie či je sekcia daleko vzadu
            if(sekcie[i].transform.position.z - hracAutoTransform.position.z < -dlzkaSekcie){
                //Uloži poziciu sekcie a vypne ju
                Vector3 poslednaPoziciaSekcie = sekcie[i].transform.position;
                sekcie[i].SetActive(false);

                //Ziska novu sekciu zapne ju a posunie vpred
                sekcie[i] = ZiskajNahoduSekciu();

                //Posunie novu sekciu na miesto a aktivuje ju
                sekcie[i].transform.position = new Vector3(poslednaPoziciaSekcie.x, -10, poslednaPoziciaSekcie.z + dlzkaSekcie * sekcie.Length);
                sekcie[i].SetActive(true);
            }
        }
    }

    GameObject ZiskajNahoduSekciu(){
        //Vyberie nahodny index 
        int nahodnyIndex = Random.Range(0, sekciePole.Length);

        bool najdenaNovaSekcia = false;

        while(!najdenaNovaSekcia){
            //Ak je bool na false, nasli sme novu sekciu
            if(!sekciePole[nahodnyIndex].activeInHierarchy){
                najdenaNovaSekcia = true;
            }
            else{
                //Ak je bool na true, musime najst novu tak zvyšime index
                nahodnyIndex++;

                if(nahodnyIndex > sekciePole.Length - 1){
                    nahodnyIndex = 0;
                }
            }
        }

        return sekciePole[nahodnyIndex];
    }
    
}
