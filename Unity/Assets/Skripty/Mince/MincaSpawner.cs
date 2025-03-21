using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MincaSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject mincaPrefab; //prefab mince ktorý priradíme v unity inspectore lebo je [SerializeField]

    [SerializeField]
    Transform hracTransform;    //poloha hráča v hernom priestore

    [SerializeField]
    float intervalSpawnu = 2.0f;    //každé 2 sekundy vytvorí skript novú mincu

    [SerializeField]
    float vzdialenostSpawnuPredHracom = 50.0f;  //ako daleko pred hráčom spawnne novú mincu

    private void Start()
    {
        if (hracTransform == null)
            hracTransform = GameObject.FindGameObjectWithTag("Player").transform;   //nájdeme hráča pomocou tagu

        StartCoroutine(SpawnujMinceCO());   //spustenie korutiny na spawnovanie mincí
    }

    IEnumerator SpawnujMinceCO()
    {
        while (true)
        {
            SpawnMince();
            yield return new WaitForSeconds(intervalSpawnu);    //yield return vždy počká 2 sekundy kým znova spustí korutinu na spawn mince
        }
    }

    void SpawnMince()
    {

        float[] pruhy = Pruhy.PruhyAuta;    //z triedy Pruhy spraví pole
        int nahodnyIndexPruhu = Random.Range(0, pruhy.Length); //vyberie náhodný pruh ľavý/pravý v ktorom sa minca spawnne
        float pruhX = pruhy[nahodnyIndexPruhu];

        Vector3 poziciaSpawnu = new Vector3(
            pruhX,
            0.2f,
            hracTransform.position.z + vzdialenostSpawnuPredHracom
        );  // výpočet aby sa minca spawnla mierne nad zemou

        Quaternion rotacia = Quaternion.Euler(0f, 90f, 0f); //otočenie mince po osi Y, čisto pre vizuálne účely

        Instantiate(mincaPrefab, poziciaSpawnu, rotacia);   //vytvorí novú mincu v scéne s daným miestom a orientáciou
    }
}