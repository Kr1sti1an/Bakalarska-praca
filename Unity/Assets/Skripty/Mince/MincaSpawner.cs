using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MincaSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject mincaPrefab;

    [SerializeField]
    Transform hracTransform;

    [SerializeField]
    float intervalSpawnu = 2.0f;

    [SerializeField]
    float vzdialenostSpawnuPredHracom = 50.0f;

    private void Start()
    {
        if (hracTransform == null)
            hracTransform = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(SpawnujMinceCO());
    }

    IEnumerator SpawnujMinceCO()
    {
        while (true)
        {
            SpawnMince();
            yield return new WaitForSeconds(intervalSpawnu);
        }
    }

    void SpawnMince()
    {

        float[] pruhy = Pruhy.PruhyAuta;
        int nahodnyIndexPruhu = Random.Range(0, pruhy.Length);
        float pruhX = pruhy[nahodnyIndexPruhu];

        Vector3 poziciaSpawnu = new Vector3(
            pruhX,
            0.2f,
            hracTransform.position.z + vzdialenostSpawnuPredHracom
        );

        Quaternion rotacia = Quaternion.Euler(0f, 90f, 0f);

        Instantiate(mincaPrefab, poziciaSpawnu, rotacia);
    }
}