using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ArduinoVstupManazer : MonoBehaviour
{
    //verejné premenné, aby sa dali nastaviť v Unity editore, určujú z ktorého sériového portu sa má čitať a akou rýchlosťou (baudrate)
    public string nazovPortu = "/dev/cu.usbmodem1101";
    public int baudovaRychlost = 9600;

    private SerialPort seriovyPort; //inštancia SerialPort cez ktorú prebieha komunikácia s Arduinom
    private Thread citacieVlakno;   //samostatné vlákno, v ktorom neustále čítame dáta aby sme sa vyhli blokovaniu hlavného vlákna
    private bool bezi;

    private string prijateData = "";    //pomocná premenná do ktorej vlákno ukladá posledné prijaté dáta, v Update() sa potom spracujú

    void Start()
    {
        OtvorSpojenie();    //hneď v metóde start() sa pripájame k sériovému portu
    }

    void OtvorSpojenie()
    {
        Debug.Log("Snažím sa otvoriť port: " + nazovPortu);

        seriovyPort = new SerialPort(nazovPortu, baudovaRychlost)   //konštruktor
        {
            ReadTimeout = 50    //časový limit na čítanie
        };

        try
        {
            seriovyPort.Open();
            bezi = true;
            citacieVlakno = new Thread(CitajSerial);//priradíme funkciu CitajSerial ako vstupný bod
            citacieVlakno.Start();  //tu už reálne spustíme vlákno

            Debug.Log("Arduino ovládač úspešne pripojený!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Nemôžem otvoriť sériový port: " + e.Message);
        }
    }

    void CitajSerial()
    {
        while (bezi && seriovyPort != null && seriovyPort.IsOpen)   //toto vlákno bude bežať kým je sériový port stále otvorený
        {
            try
            {
                string riadok = seriovyPort.ReadLine(); //čaká na jeden riadok ukončneý \n z Arduina(v arduino kóde Serial.print(hodnota))
                lock (prijateData)
                {
                    prijateData = riadok;   //zamkneme retazeprijateData a priradíme doňho aktuálny riadok. lock sa používa preto aby sa predišlo kolíziam ked sa v Update() táto premenná zároveň číta
                }
            }
            catch (System.TimeoutException)
            {
            }
            catch (System.Exception e)  //ak port prestane existovať vypíše sa error
            {
                Debug.LogError("Chyba pri čítaní zo sériového portu: " + e.Message);
            }
        }
    }

    void Update()
    {
        string lokalneData = "";    //každý frame sa vytvorí premnná lokalneData
        lock (prijateData)
        {
            lokalneData = prijateData;  //pomocou lock sa reťazec bezpečne prečíta, lokalneData naplníme a prijateData vyčistíme
            prijateData = "";
        }

        if (!string.IsNullOrEmpty(lokalneData)) //ak premenná nieje prázdna tak sa volá SpracujData(), reťazec sa rozdelí na hodnoty pre zatáčanie a tlačidlá
        {
            SpracujData(lokalneData);
        }
    }

    void SpracujData(string data)
    {
        string[] hodnoty = data.Split(','); //rozdelí prichádzajúci retazec podla čiarky formát "pot,tlacidlo1,tlacidlo2"
        if (hodnoty.Length == 3)    //presne tri segmenty: potenciometer, a dve tlacidlá
        {
            int potHodnota = int.Parse(hodnoty[0].Trim());  //0 - 1023
            bool tlacidloDopredu = hodnoty[1].Trim() == "1"; //zistíme či je stalčené 0 / 1
            bool tlacidloBrzda = hodnoty[2].Trim() == "1";  //zistíme či je stalčené 0 / 1

            float zatacanie = Map(potHodnota, 0, 1023, -1f, 1f); //premapujeme rozsah 0-1023 na strane Arduina na -1 až 1 na strane Unity

            if (VstupManazer.Instance != null)
                VstupManazer.Instance.NastavVstupZArduina(zatacanie, tlacidloDopredu, tlacidloBrzda); //do VstupManazer odošleme zatáčanie a stavy tlačidiel, aby mohol dalej riadiť auto
        }
    }

    float Map(float hodnota, float od1, float do1, float od2, float do2)    //premapovanie hodnôt z jedného intervalu do druhého
    {
        return Mathf.Clamp(
            (hodnota - od1) * (do2 - od2) / (do1 - od1) + od2,  //Mathf.Clamp aby sa neprekročil cieľový interval
            od2,
            do2
        );
    }

    void OnDestroy()    // ked sa skript alebo objekt ničí
    {
        bezi = false;   //vlákno CitajSerial opustí slučku
        if (citacieVlakno != null && citacieVlakno.IsAlive)
        {
            citacieVlakno.Join();   //Join() počká kým sa vlákno bezpečne ukončí
        }

        if (seriovyPort != null && seriovyPort.IsOpen)
        {
            seriovyPort.Close();
            Debug.Log("Sériový port zatvorený v OnDestroy.");
        }
    }

    void OnApplicationQuit()    //ked sa ukončuje hra
    {
        bezi = false;
        if (citacieVlakno != null && citacieVlakno.IsAlive)
        {
            citacieVlakno.Join();
        }

        if (seriovyPort != null && seriovyPort.IsOpen)
        {
            seriovyPort.Close();
            Debug.Log("Sériový port zatvorený v OnApplicationQuit.");
        }
    }
}