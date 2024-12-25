using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SekciaManazer : MonoBehaviour
{
    Transform hracAutoTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Nájdeme herný objekt s tagom "Player" a uložíme jeho transformáciu
        hracAutoTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Vypočíta vzdialenosť medzi aktuálnym objektom a hráčom pozdĺž osi Z
        float vzdialenostOdHraca = transform.position.z - hracAutoTransform.position.z;

        float lerpPerecentualnost = 1.0f - ((vzdialenostOdHraca - 100) / 150.0f);

        // Obmedzíme hodnotu na rozsah medzi 0.0 a 1.0
        lerpPerecentualnost = Mathf.Clamp01(lerpPerecentualnost);

        transform.position = Vector3.Lerp(new Vector3(transform.position.x, -10, transform.position.z), new Vector3(transform.position.x, 0, transform.position.z), lerpPerecentualnost);
    }
}
