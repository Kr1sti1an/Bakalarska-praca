using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VybuchManazer : MonoBehaviour
{

    [SerializeField]
    GameObject originalnyObjekt;

    [SerializeField]
    GameObject model;

    Rigidbody[] rigidbodies;

    private void Awake(){
        rigidbodies = model.GetComponentsInChildren<Rigidbody>(true);
    } 

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Vybuch(Vector3 vonkajsiaSila){
        originalnyObjekt.SetActive(false);

        foreach (Rigidbody rb in rigidbodies){
            rb.transform.parent = null;

            rb.GetComponent<MeshCollider>().enabled = true;

            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(Vector3.up * 200 + vonkajsiaSila, ForceMode.Force);
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);

            //Zmenim tag aby vybuchli AI auta po zasahu casti z mojho auta
            rb.gameObject.tag = "AutoCasti";
        }
    }
}
