using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Concurrent;
using System;

public class SerialController : MonoBehaviour
{
    [Header("Serial Settings")]
    [SerializeField] private string portName = "/dev/cu.usbmodem64E83369A5242"; 
    [SerializeField] private int baudRate = 115200;
    [SerializeField] private GameObject pieceGO;

    private SerialPort serialPort;
    private Thread serialThread;
    private bool running;
    private ConcurrentQueue<string> queue = new();

    public static string command = ""; 

    void Start()
    {
        if (string.IsNullOrEmpty(portName))
        {
            Debug.LogError("No serial port selected! Check the Inspector.");
            return;
        }

        try {
            // Setting ReadTimeout to 50ms to keep the thread responsive
            serialPort = new SerialPort(portName, baudRate) { ReadTimeout = 50, DtrEnable = true, RtsEnable = true };
            serialPort.Open();
            
            running = true;
            serialThread = new Thread(ReadSerial);
            serialThread.Start();
            Debug.Log("Serial Port " + portName + " opened successfully.");
        }
        catch (Exception e) {
            Debug.LogError("Serial Port Error: " + e.Message);
        }
    }

    void Update()
    {   
        while (queue.TryDequeue(out string line))
        {
            SerialController.command = line.Trim();
            
            // Logic to find the active piece currentlPiecey falling
            // Piece activePiece = FindObjectOfType<Piece>();
            //Piece activePiece = pieceGO.GetComponent<Piece>();

            // if (activePiece != null)
            // {
            //     if (cmd.Equals("ROTATE")) {
            //         command = cmd;
            //        // activePiece.Rotate(1); // Triggers the public Rotate method
            //         //Debug.Log("MPU ACTION: Rotate");
            //     }
            //     else if (cmd.Equals("DROP")) {
            //         // command = cmd;
            //         //activePiece.HardDrop(); // Triggers the public HardDrop method
            //         // Debug.Log("MPU ACTION: Hard Drop");
            //     }
            //     else{
            //         command = "";
            //         // Debug.Log("MPU ACTION: NOTHING");
            //     }
            // }
        }
    }

    void OnDestroy()
    {
        running = false;
        if (serialThread != null && serialThread.IsAlive) serialThread.Join();
        if (serialPort != null && serialPort.IsOpen) serialPort.Close();
    }

    void ReadSerial()
    {
        while (running && serialPort.IsOpen)
        {
            try {
                string line = serialPort.ReadLine();
                queue.Enqueue(line);
            }
            catch (TimeoutException) {
                // Ignore timeouts to keep the console clean
            }
            catch (Exception e) {
                Debug.LogWarning("Thread Read Error: " + e.Message);
            }
        }
    }
}


// using UnityEngine;
// using System.IO.Ports;
// using System.Threading;
// using System.Collections.Concurrent;
// using System;

// public class SerialController : MonoBehaviour
// {
//     [Header("Serial Settings")]
//     [SerializeField] private string portName = "/dev/cu.usbmodem64E83369A5242"; 
//     [SerializeField] private int baudRate = 9600;

//     private SerialPort serialPort;
//     private Thread serialThread;
//     private bool running;
//     private ConcurrentQueue<string> queue = new();

//     void Start()
//     {
//         if (string.IsNullOrEmpty(portName))
//         {
//             Debug.LogError("No serial port selected! Check the Inspector.");
//             return;
//         }

//         try {
//             serialPort = new SerialPort(portName, baudRate) { ReadTimeout = 50, DtrEnable = true, RtsEnable = true };
//             serialPort.Open();
            
//             running = true;
//             serialThread = new Thread(ReadSerial);
//             serialThread.Start();
//             Debug.Log("Serial Port " + portName + " opened at " + baudRate);
//         }
//         catch (Exception e) {
//             Debug.LogError("Serial Port Error: " + e.Message);
//         }
        
//     }
    

//     // void Update()
//     // {   
//     //     // Process messages from the background thread
//     //     while (queue.TryDequeue(out string line))
//     //     {
            
//     //         string cmd = line.Trim();
            
//     //         // Find the active Tetris piece script
//     //         Piece activePiece = FindObjectOfType<Piece>();

//     //         if (activePiece != null)
//     //         {
//     //             if (cmd == "ROTATE") {
//     //                 activePiece.Rotate(1);
//     //                 Debug.Log("MPU: Executing Rotate");
//     //             }
//     //             else if (cmd == "DROP") {
//     //                 activePiece.HardDrop();
//     //                 Debug.Log("MPU: Executing Hard Drop");
//     //             }
//     //         }
//     //     }
//     // }

//     void OnDestroy()
//     {
//         running = false;
//         if (serialThread != null && serialThread.IsAlive) serialThread.Join();
//         if (serialPort != null && serialPort.IsOpen) serialPort.Close();
//     }

//     void ReadSerial()
//     {
//         Debug.Log(running + "/" + serialPort.IsOpen);
//         while (running && serialPort.IsOpen)
//         {
//             string line = serialPort.ReadLine();
//             try {
//                 queue.Enqueue(line);
//             }
//             catch (TimeoutException e) {
//                 Debug.LogWarning("Timeout Exception: " + e.Message);
//              }
//             catch (Exception e) {
//                 Debug.LogWarning("Thread Read Error: " + e.Message);
//             }
//         }
//     }
// }