using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;

public class Client : MonoBehaviour {
    public string clientName;
    public bool isHost;
    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private List<gameClient> players = new List<gameClient>();
    private void Start() {
        DontDestroyOnLoad(gameObject);
    }
    public bool connectToServer(string host, int port) {
        if (socketReady)
            return false;
        try {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            socketReady = true;
        } catch (System.Exception e) {
            Debug.Log("Socket Error :" + e.Message);
        }
        return socketReady;
    }
    private void Update() {
        if (socketReady) {
            if (stream.DataAvailable) {
                string data = reader.ReadLine();
                if (data != null)
                    onIncomingData(data);
            }
        }
    }

    //Sending messages to  the server
    public void send(string data) {
        if (!socketReady)
            return;
        writer.WriteLine(data);
        writer.Flush();
    }

    //Read messages from the server
    private void onIncomingData(string data) {
        Debug.Log("CLIENT: " + data);
        string[] aData = data.Split('|');
        switch (aData[0]) {
            case "SWHO":
                for (int i = 1; i < aData.Length - 1; i++) {
                    userConnected(aData[i], false);
                }
                send("CWHO|" + clientName + "|" + ((isHost ? 1 : 0).ToString()));
                break;
            case "SCNN":
                userConnected(aData[1], false);
                break;
            case "SMOV":
                BoardManager.Instance.moveChessPiece(int.Parse(aData[1]), int.Parse(aData[2]), int.Parse(aData[3]), int.Parse(aData[4]));
                break;
        }
    }
    private void userConnected(string name, bool host) {
        gameClient c = new gameClient();
        c.name = name;
        players.Add(c);
        if (players.Count == 2)
            GameManager.Instance.startGame();
    }
    private void OnApplicationQuit() {
        closeSocket();
    }
    private void OnDisable() {
        closeSocket();
    }
    private void closeSocket() {
        if (!socketReady)
            return;
        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
}
public class gameClient {
    public string name;
    public bool isHost;
}
