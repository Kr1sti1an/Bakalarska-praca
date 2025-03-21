using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VybuchManazer : MonoBehaviour
{
    [SerializeField]
    GameObject originalnyObjekt;    //odkaz na pôvodný objekt v celku pred výbuchom

    [SerializeField]
    GameObject model;   //obsahuje deti s Rigidbody komponentami, ktoré predstavuju časti auta

    Rigidbody[] telesa; //pole do ktorého sa pri Awake načítajú všetky Rigidbody komponenty

    private void Awake()
    {
        telesa = model.GetComponentsInChildren<Rigidbody>(true);    //vyhladá všetky Rigidbody komponenty v model vrátane neaktívnych objektov 
    }

    void Start()
    {
    }

    public void Vybuch(Vector3 vonkajsiaSila)
    {
        originalnyObjekt.SetActive(false);  //vypneme pôvodný objekt, súvislú karosériu auta

        foreach (Rigidbody rb in telesa)    //pre každý Rigidbody 
        {
            rb.transform.parent = null; // zruší parenting

            rb.GetComponent<MeshCollider>().enabled = true; //zapne kolíziu aby časti mohli narážať do prostredia

            rb.gameObject.SetActive(true);
            rb.isKinematic = false; //vypneme kinematiku aby gravitácia mohla pôsobiť
            rb.interpolation = RigidbodyInterpolation.Interpolate;  //plynulejší pohyb
            rb.AddForce(Vector3.up * 200 + vonkajsiaSila, ForceMode.Force); //sila smerom nahor
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);    //pridá náhodný rotačný impulz aby

            rb.gameObject.tag = "AutoCasti";    //mení tag na úlomky aby rozbili dalšie autá
        }

        StartCoroutine(NacitajGameOver());  //pridáme korutinu ktorá počka a následne spustí GameOver scénu
    }

    IEnumerator NacitajGameOver()
    {
        yield return new WaitForSeconds(2f);    //korutina počká 2 sekundy
        SceneManager.LoadScene("GameOver");
    }
}