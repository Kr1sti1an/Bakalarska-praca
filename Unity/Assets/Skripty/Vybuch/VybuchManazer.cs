using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VybuchManazer : MonoBehaviour
{
    [SerializeField]
    GameObject originalnyObjekt;

    [SerializeField]
    GameObject model;

    Rigidbody[] telesa;

    private void Awake()
    {
        telesa = model.GetComponentsInChildren<Rigidbody>(true);
    }

    void Start()
    {
    }

    public void Vybuch(Vector3 vonkajsiaSila)
    {
        originalnyObjekt.SetActive(false);

        foreach (Rigidbody rb in telesa)
        {
            rb.transform.parent = null;

            rb.GetComponent<MeshCollider>().enabled = true;

            rb.gameObject.SetActive(true);
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.AddForce(Vector3.up * 200 + vonkajsiaSila, ForceMode.Force);
            rb.AddTorque(Random.insideUnitSphere * 0.5f, ForceMode.Impulse);

            rb.gameObject.tag = "AutoCasti";
        }

        StartCoroutine(NacitajGameOver());
    }

    IEnumerator NacitajGameOver()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }
}