using UnityEngine;
using System.IO.Ports; 
using System.Threading;

public class ArduinoInputManager : MonoBehaviour
{
    public string portName = "/dev/cu.usbmodem1101"; 
    public int baudRate = 9600;

    private SerialPort serialPort;
    private Thread readThread;
    private bool isRunning;

    private string receivedData = "";

    void Start()
    {
        OpenConnection();
    }

    void OpenConnection()
    {
        Debug.Log("Snažím sa otvoriť port: " + portName); 

        serialPort = new SerialPort(portName, baudRate)
        {
            ReadTimeout = 50
        };

        try
        {
            serialPort.Open();
            isRunning = true;
            readThread = new Thread(ReadSerial);
            readThread.Start();

            Debug.Log("Arduino ovládač úspešne pripojený!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Nemôžem otvoriť sériový port: " + e.Message);
        }
    }

    void ReadSerial()
    {
        while (isRunning && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string line = serialPort.ReadLine();
                lock (receivedData)
                {
                    receivedData = line;
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
        string data = "";
        lock (receivedData)
        {
            data = receivedData;
            receivedData = "";
        }

        if (!string.IsNullOrEmpty(data))
        {
            ProcessData(data);
        }
    }

    void ProcessData(string data)
    {
        
        string[] values = data.Split(',');
        if (values.Length == 3)
        {
            int potValue = int.Parse(values[0].Trim());
            bool forwardButton = values[1].Trim() == "1";
            bool brakeButton = values[2].Trim() == "1";

            
            float steering = Map(potValue, 0, 1023, -1f, 1f);

            
            VstupManazer.Instance.NastavVstupZArduina(steering, forwardButton, brakeButton);
        }
    }

    float Map(float value, float from1, float to1, float from2, float to2)
    {
        return Mathf.Clamp((value - from1)*(to2 - from2)/(to1 - from1)+from2, from2, to2);
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (readThread != null)
            readThread.Join();

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}