using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NahodneObjekty : MonoBehaviour
{
    [SerializeField]
    Vector3 lokalnaRotaciaMin = Vector3.zero;
    [SerializeField]
    Vector3 lokalnaRotaciaMax = Vector3.zero;

    [SerializeField]
    float lokalnaStupnicaNasobitelMin = 0.8f;
    [SerializeField]
    float lokalnaStupnicaNasobitelMax = 1.5f;

    Vector3 localScaleOriginal = Vector3.one;

    private void Start(){
        localScaleOriginal = transform.localScale;
    }
    
    void OnEnable()
    {
        transform.localRotation = Quaternion.Euler(Random.Range(lokalnaRotaciaMin.x, lokalnaRotaciaMax.x), Random.Range(lokalnaRotaciaMin.y, lokalnaRotaciaMax.y), Random.Range(lokalnaRotaciaMin.z, lokalnaRotaciaMax.z));

        transform.localScale = localScaleOriginal * Random.Range(lokalnaStupnicaNasobitelMin, lokalnaStupnicaNasobitelMax);
    }
}
