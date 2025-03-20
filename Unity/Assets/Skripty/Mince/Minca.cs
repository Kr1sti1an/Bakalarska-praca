using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 50f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Minca získaná!");
            SkoreManazer.PridajSkore(1);
            Destroy(gameObject);
        }
    }
}