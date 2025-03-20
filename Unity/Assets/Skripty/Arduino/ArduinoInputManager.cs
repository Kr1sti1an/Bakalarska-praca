using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class ArduinoVstupManazer : MonoBehaviour
{
    public string nazovPortu = "/dev/cu.usbmodem1101";
    public int baudovaRychlost = 9600;

    private SerialPort seriovyPort;
    private Thread citacieVlakno;
    private bool bezi;

    private string prijateData = "";

    void Start()
    {
        OtvorSpojenie();
    }

    void OtvorSpojenie()
    {
        Debug.Log("Snažím sa otvoriť port: " + nazovPortu);

        seriovyPort = new SerialPort(nazovPortu, baudovaRychlost)
        {
            ReadTimeout = 50
        };

        try
        {
            seriovyPort.Open();
            bezi = true;
            citacieVlakno = new Thread(CitajSerial);
            citacieVlakno.Start();

            Debug.Log("Arduino ovládač úspešne pripojený!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Nemôžem otvoriť sériový port: " + e.Message);
        }
    }

    void CitajSerial()
    {
        while (bezi && seriovyPort != null && seriovyPort.IsOpen)
        {
            try
            {
                string riadok = seriovyPort.ReadLine();
                lock (prijateData)
                {
                    prijateData = riadok;
                }
            }
            catch (System.TimeoutException)
            {
            }
            catch (System.Exception e)
            {
                Debug.LogError("Chyba pri čítaní zo sériového portu: " + e.Message);
            }
        }
    }

    void Update()
    {
        string lokalneData = "";
        lock (prijateData)
        {
            lokalneData = prijateData;
            prijateData = "";
        }

        if (!string.IsNullOrEmpty(lokalneData))
        {
            SpracujData(lokalneData);
        }
    }

    void SpracujData(string data)
    {
        string[] hodnoty = data.Split(',');
        if (hodnoty.Length == 3)
        {
            int potHodnota = int.Parse(hodnoty[0].Trim());
            bool tlacidloDopredu = hodnoty[1].Trim() == "1";
            bool tlacidloBrzda = hodnoty[2].Trim() == "1";

            float zatacanie = Map(potHodnota, 0, 1023, -1f, 1f);

            if (VstupManazer.Instance != null)
                VstupManazer.Instance.NastavVstupZArduina(zatacanie, tlacidloDopredu, tlacidloBrzda);
        }
    }

    float Map(float hodnota, float od1, float do1, float od2, float do2)
    {
        return Mathf.Clamp(
            (hodnota - od1) * (do2 - od2) / (do1 - od1) + od2,
            od2,
            do2
        );
    }

    void OnDestroy()
    {
        bezi = false;
        if (citacieVlakno != null && citacieVlakno.IsAlive)
        {
            citacieVlakno.Join();
        }

        if (seriovyPort != null && seriovyPort.IsOpen)
        {
            seriovyPort.Close();
            Debug.Log("Sériový port zatvorený v OnDestroy.");
        }
    }

    void OnApplicationQuit()
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