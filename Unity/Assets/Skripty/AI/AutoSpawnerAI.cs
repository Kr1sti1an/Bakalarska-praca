using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpawnerAI : MonoBehaviour
{

    [SerializeField]
    GameObject[] autoAIPrefaby;

    GameObject[] autoAIPole = new GameObject[20];

    Transform hracAutoTransform;

    //Casovanie
    float casOdPoslednehoSpawnu = 0;
    WaitForSeconds cakaj = new WaitForSeconds(0.5f);

    //Kontrola predbehnutia
    [SerializeField]
    LayerMask ostatneAutaLayerMaska;

    Collider[] kontrolaPredbehnutiaCollider = new Collider[1];

    // Start is called before the first frame update
    void Start()
    {

        hracAutoTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int prefabIndex = 0;

        for (int i = 0; i < autoAIPole.Length; i++)
        {
            autoAIPole[i] = Instantiate(autoAIPrefaby[prefabIndex]);
            autoAIPole[i].SetActive(false);

            prefabIndex++;

            //Loop cez prefab index ak sa nám minú prefaby
            if (prefabIndex > autoAIPrefaby.Length - 1)
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
            CistenieAutZaKamerou();
            SpawnNovehoauta();
            yield return cakaj;
        }
    }

    void SpawnNovehoauta()
    {
        if (Time.time - casOdPoslednehoSpawnu < 2)
        {
            return;
        }

        GameObject autoNaSpawn = null;

        //Najdeme auto na spawn
        foreach (GameObject aiAuto in autoAIPole)
        {
            //Preskoč aktivne auta
            if (aiAuto.activeInHierarchy)
            {
                continue;
            }

            autoNaSpawn = aiAuto;
            break;
        }

        //Ak neni auto k dispozicií na spawn
        if (autoNaSpawn == null)
        {
            return;
        }

        Vector3 spawnPozicia = new Vector3(0, 0, hracAutoTransform.transform.position.z + 100);

        if (Physics.OverlapBoxNonAlloc(spawnPozicia, Vector3.one * 2, kontrolaPredbehnutiaCollider, Quaternion.identity, ostatneAutaLayerMaska) > 0)
        {
            return;
        }

        autoNaSpawn.transform.position = spawnPozicia;
        autoNaSpawn.SetActive(true);

        casOdPoslednehoSpawnu = Time.time;
    }

    void CistenieAutZaKamerou()
    {
        foreach (GameObject aiAuto in autoAIPole)
        {
            //Preskocime neaktivne auta
            if (!aiAuto.activeInHierarchy)
            {
                continue;
            }

            //Skontrolujeme či je AI Auto dost daleko
            if (aiAuto.transform.position.z - hracAutoTransform.position.z > 200)
            {
                aiAuto.SetActive(false);
            }

            //Skontrolujeme či je AI Auto dost daleko
            if (aiAuto.transform.position.z - hracAutoTransform.position.z < -50)
            {
                aiAuto.SetActive(false);
            }
        }
    }
}
