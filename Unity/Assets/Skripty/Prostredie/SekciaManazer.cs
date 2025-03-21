using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekciaManazer : MonoBehaviour
{
    Transform hracAutoTransform;    //hráčová poloha

    // Start is called before the first frame update
    void Start()
    {
        hracAutoTransform = GameObject.FindGameObjectWithTag("Player").transform;   //nájdme objekt s tagom hráčovho auta
    }

    // Update is called once per frame
    void Update()
    {
        float vzdialenostOdHraca = transform.position.z - hracAutoTransform.position.z; //meriame vzdialenosť sekcie od hráča po osi Z ak je pred hráčom kladen ak za hráčom tak záporne

        float lerpPerecentualnost = 1.0f - ((vzdialenostOdHraca - 100) / 150.0f);   //tu počítame interpoláciu v rozmedzí 0 až 1

        lerpPerecentualnost = Mathf.Clamp01(lerpPerecentualnost);   //oklieštime výsledok do intervalu 0 až 1

        transform.position = Vector3.Lerp(new Vector3(transform.position.x, -10, transform.position.z), new Vector3(transform.position.x, 0, transform.position.z), lerpPerecentualnost);   //určuje ako velmi je sekcia zdvihnutá od -10 po 0
    }
}
