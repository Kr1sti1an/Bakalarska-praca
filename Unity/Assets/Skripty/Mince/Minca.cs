using UnityEngine;

public class Minca : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 50f);   //objekt Mince sa sám zničí aby v scéne neostával príliš dlho ked sa nevezme
    }

    private void OnTriggerEnter(Collider other) //táto metóda sa zavolá ked collider objektu s isTrigger zistí prienik s iným Colliderom
    {
        if (other.CompareTag("Player")) //ak po minci prejde hráč sa pridá skóre a zruší minca
        {
            SkoreManazer.PridajSkore(1);
            Destroy(gameObject);
        }
    }
}