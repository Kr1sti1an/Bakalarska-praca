using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoManazer : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Transform hernyModel;   //vizuálny model auta, napríklad pre natáčanie modelu

    [SerializeField]
    MeshRenderer autoSietovyCitac;  //mesh renderer, použitý napríklad na zmenu farby svetiel pri brzdení

    [SerializeField]
    VybuchManazer vybuchManazer;    //skript ktorý spracováva logiku výbuchu, auto sa rozletí na 8častí

    //Max hodnoty pre rýchlosť a zabáčanie
    float maxOvladaciaRychlost = 2;
    float maxVpredRychlost = 30;

    //Násobitele, určujú aký silný bude plyn, brzda a zatáčanie
    float rozbehNasobitel = 3;
    float brzdaNasobitel = 15;
    float volantNasobitel = 5;

    // tu sa ukladá vstup od hráča X/Y os lebo Vector2
    Vector2 vstup = Vector2.zero;

    int _EmissionColor = Shader.PropertyToID("_EmissionColor"); //emisívna hodnota, slúži na zmenu emission color farby na svetlách, ked brzdím svietia na červeno viac
    Color emisivnaFarba = Color.white;
    float emisivnaFarbaNasobitel = 0f;

    bool jeVybuchnuty = false;  //stav výbuchu
    bool jeHrac = true; // rozdeil či je auto hráčove alebo AI

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
            return; //ak vybuchne koniec nerobí sa logika otáčania auta ani nič
        }

        hernyModel.transform.rotation = Quaternion.Euler(0, rb.linearVelocity.x * 5, 0);    //ak auto zatočí napríklad doprava vyzreá to že auto sa reálne natáča doprava

        if (autoSietovyCitac != null)
        {
            float nasobitelFarbyAuta = 0f;

            if (vstup.y < 0)
            {
                nasobitelFarbyAuta = 4.0f;  //vačšia žiara pri brzdení ak stlačíme brzdu vstup.y < 0
            }

            emisivnaFarbaNasobitel = Mathf.Lerp(emisivnaFarbaNasobitel, nasobitelFarbyAuta, Time.deltaTime * 4);    //časom sa vyblednuje emisívna hodnota, plynulý prechod brzdových svetiel

            autoSietovyCitac.material.SetColor(_EmissionColor, emisivnaFarba * emisivnaFarbaNasobitel); //aplikujeme nové zafarbenie na materiál
        }

    }

    private void FixedUpdate()  //FixedUpdate(), volá sa v konštantom čase nezávisle od FPS ako normálny Update() ktorý sa volá každý frame. FixedUpdate() je v tomto prípade lepší vzhladom pre prácu s fyzikou
    {

        //Je vybuchnuty
        if (jeVybuchnuty)
        {
            rb.linearDamping = rb.linearVelocity.z * 0.1f;
            rb.linearDamping = Mathf.Clamp(rb.linearDamping, 1.5f, 10);

            rb.MovePosition(Vector3.Lerp(transform.position, new Vector3(0, 0, transform.position.z), Time.deltaTime * 0.5f)); //pohyb vpred po výbuchu auta

            return;
        }

        if (vstup.y > 0)    //volá sa rozbeh ak je stlačený pushbutton
        {
            Rozbeh();
        }
        else
        {
            rb.linearDamping = 0.2f;    //ak sa nedrží plyn auto spomaľuje ale nie uplne prudko
        }

        if (vstup.y < 0)    //volá sa brzda
        {
            Brzda();
        }

        Volant();   //tu sa rieši bočný pohyb na základe výstupov z potenciometra cez arduino

        if (rb.linearVelocity.z <= 0)   //zákaz cúvania auta, maximálne len zabrzdí na nulu
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    void Rozbeh()
    {
        rb.linearDamping = 0;   //auto sa rozbehne plynule

        if (rb.linearVelocity.z >= maxVpredRychlost)    //udržanie v rýchlostnom limite
        {
            return;
        }

        rb.AddForce(rb.transform.forward * rozbehNasobitel * vstup.y);  //ak ešte ideme pomaly môžeme zrýchliť
    }

    void Brzda()
    {
        if (rb.linearVelocity.z <= 0)   //auto brzdí len ak sa hýbe vpred, inak už nie
        {
            return;
        }
        rb.AddForce(rb.transform.forward * brzdaNasobitel * vstup.y);
    }

    void Volant()
    {
        if (Mathf.Abs(vstup.x) > 0)
        {
            float rychlostnyZakladnyLimit = rb.linearVelocity.z / 5.0f;
            rychlostnyZakladnyLimit = Mathf.Clamp01(rychlostnyZakladnyLimit);   //oklieštime pomer na 0 až 1 pomoocu Mathf.Clamp01()

            rb.AddForce(rb.transform.right * volantNasobitel * vstup.x * rychlostnyZakladnyLimit);  //čím auto ide rýchlejšie, tým viac možem otáčať potenciometer, pridáva sa bočná sila a uto nezatáča pri vysokej rýchlosti tak prudko do strán

            float normalizovaneX = rb.linearVelocity.x / maxOvladaciaRychlost; //obmedzenie X rýchlosti zatáčania

            normalizovaneX = Mathf.Clamp(normalizovaneX, -1.0f, 1.0f);  //Oklieštenie na -1 až 1

            rb.linearVelocity = new Vector3(normalizovaneX * maxOvladaciaRychlost, 0, rb.linearVelocity.z); //bočná rýchlosť nepresiahne nami definovaný limit maxovladaciaRychlost
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, new Vector3(0, 0, rb.linearVelocity.z), Time.fixedDeltaTime * 3);   //automatické vycentrovanie auta ak nezatáčame
        }
    }

    public void NastavVstup(Vector2 vstupnyVektor)
    {
        vstupnyVektor.Normalize();  //zabezpečí že vstupný vektor je 1 ak bol nenulový, XY má vždy dĺžku 1
        vstup = vstupnyVektor;
    }

    public void NastavMaxRychlost(float novaMaxRychlost)
    {
        maxVpredRychlost = novaMaxRychlost; //umožnuje meniť maximálnu rýchlosť dopredu
    }

    IEnumerator SpomalCasCO()   //korutina na spomalenie času
    {
        while (Time.timeScale > 0.2f)
        {
            Time.timeScale -= Time.deltaTime * 2;   //efekt spomalenia

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);  //yield return, počká 0.5 sekundy 

        while (Time.timeScale <= 1.0f)
        {
            Time.timeScale += Time.deltaTime;   //čas sa znova vráti do normálu

            yield return null;
        }

        Time.timeScale = 1.0f;
    }

    //Eventy
    private void OnCollisionEnter(Collision collision)
    {

        if (!jeHrac)    //ak je to AIAuto tak vybuchne len pri kolízií s hráčom alebo časťou z hráča, súčiastky ktoré vyleteli pri výbuchu, alebo častou z vybuchnuteho AI
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
        vybuchManazer.Vybuch(velocity * 45);    //rozletí sa na súčiastky

        jeVybuchnuty = true;    //nastavíme true čo mení správanie v update() a FixedUpdate()

        StartCoroutine(SpomalCasCO());  //pri výbuchu sa spomalí čas
    }

}