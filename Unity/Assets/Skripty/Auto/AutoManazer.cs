using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoManazer : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform hernyModel;

    [SerializeField]
    MeshRenderer autoSietovyCitac;

    [SerializeField]
    VybuchManazer vybuchManazer;

    //Max hodnoty
    float maxOvladaciaRychlost = 2;
    float maxVpredRychlost = 30;

    //Nasobitel
    float rozbehNasobitel = 3;
    float brzdaNasobitel = 15;
    float volantNasobitel = 5;

    //Vstup
    Vector2 vstup = Vector2.zero;

    //Emisivna hodnota
    int _EmissionColor = Shader.PropertyToID("_EmissionColor");
    Color emisivnaFarba = Color.white;
    float emisivnaFarbaNasobitel = 0f;

    //Stav vybuchu
    bool jeVybuchnuty = false;
    bool jeHrac = true;

    // Start is called before the first frame update
    void Start()
    {
        jeHrac = CompareTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (jeVybuchnuty)
        {
            return;
        }

        //Otočenie modelu auta pri zabáčaní
        hernyModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);

        if (autoSietovyCitac != null)
        {
            float nasobitelFarbyAuta = 0f;

            if (vstup.y < 0)
            {
                nasobitelFarbyAuta = 4.0f;
            }

            emisivnaFarbaNasobitel = Mathf.Lerp(emisivnaFarbaNasobitel, nasobitelFarbyAuta, Time.deltaTime * 4);

            autoSietovyCitac.material.SetColor(_EmissionColor, emisivnaFarba * emisivnaFarbaNasobitel);
        }

    }

    private void FixedUpdate()
    {

        //Je vybuchnuty
        if (jeVybuchnuty)
        {
            //Použitie dragu
            rb.linearDamping = rb.linearVelocity.z * 0.1f;
            rb.linearDamping = Mathf.Clamp(rb.linearDamping, 1.5f, 10);

            //Pohyb vpred po vybuchu auta
            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f));

            return;
        }

        //Použitie rozbehu
        if (vstup.y > 0)
        {
            Rozbeh();
        }
        else
        {
            rb.linearDamping = 0.2f;
        }

        //Použitie brzdy
        if (vstup.y < 0)
        {
            Brzda();
        }

        Volant();

        //Nedovolime aby auto cuvalo
        if (rb.linearVelocity.z <= 0)
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void Rozbeh()
    {
        rb.linearDamping = 0;

        //Zotrvanie v rychlostnom limite
        if (rb.linearVelocity.z >= maxVpredRychlost)
        {
            return;
        }

        rb.AddForce(rb.transform.forward * rozbehNasobitel * vstup.y);
    }

    void Brzda()
    {
        //Nebrzdiť pokym nejdeme v pred
        if (rb.linearVelocity.z <= 0)
        {
            return;
        }
        rb.AddForce(rb.transform.forward * brzdaNasobitel * vstup.y);
    }

    void Volant()
    {
        if (Mathf.Abs(vstup.x) > 0)
        {
            //Pohnuť autom do strany
            float rychlostnyZakladnyLimit = rb.linearVelocity.z / 5.0f;
            rychlostnyZakladnyLimit = Mathf.Clamp01(rychlostnyZakladnyLimit);

            rb.AddForce(rb.transform.right * volantNasobitel * vstup.x * rychlostnyZakladnyLimit);

            //Normalizuj X rýchlosť
            float normalizovaneX = rb.linearVelocity.x / maxOvladaciaRychlost;

            //Zaistenie že to nebude viac ako 1
            normalizovaneX = Mathf.Clamp(normalizovaneX, -1.0f, 1.0f);

            //Zaistenie že ostaneme v zakladnom rychlostnom limite
            rb.linearVelocity = new Vector3(normalizovaneX * maxOvladaciaRychlost, 0, rb.linearVelocity.z);

        }
        else
        {
            //Automaticke vycentrovanie auta
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);
        }
    }

    public void NastavVstup(Vector2 vstupnyVektor)
    {
        vstupnyVektor.Normalize();
        vstup = vstupnyVektor;
    }

    public void NastavMaxRychlost(float novaMaxRychlost)
    {
        maxVpredRychlost = novaMaxRychlost;
    }

    IEnumerator SpomalCasCO()
    {
        while (Time.timeScale > 0.2f)
        {
            Time.timeScale -= Time.deltaTime * 2;

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;

            yield return null;
        }

        Time.timeScale = 1.0f;
    }

    //Eventy
    private void OnCollisionEnter(Collision collision)
    {

        //AI auta vybuchnu len vtedy pokial sa stretnu s hracom alebo castou z hraca ktora vyleti
        if (!jeHrac)
        {
            if (collision.transform.root.CompareTag("Untagged"))
            {
                return;
            }
            if (collision.transform.root.CompareTag("AutoAI"))
            {
                return;
            }
        }

        Vector3 velocity = rb.linearVelocity;
        vybuchManazer.Vybuch(velocity * 45);

        jeVybuchnuty = true;

        StartCoroutine(SpomalCasCO());
    }

}