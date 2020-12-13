using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server : MonoBehaviour {
    public int port = 6321;
    private List<serverClient> clients;
    private List<serverClient> disconnectList;
    private TcpListener server;
    private bool serverStarted;
    public void init() {
        DontDestroyOnLoad(gameObject);
        clients = new List<serverClient>();
        disconnectList = new List<serverClient>();
        try {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            startListening();
            serverStarted = true;
        } catch (System.Exception e) {
            Debug.Log("Socket error: " + e.Message);
        }
    }
    private void Update() {
        if (!serverStarted)
            return;
        foreach (serverClient c in clients) {
            //Is the client still connected?
            if (!isConnected(c.tcp)) {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            } else {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable) {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();
                    if (data != null)
                        onIncomingData(c, data);
                }
            }
        }
        for (int i = 0; i < disconnectList.Count - 1; i++) {
            //Tell our player somebody has disconnected

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }
    //Server Send
    private void broadcast(string data, List<serverClient> cl) {
        foreach (serverClient sc in cl) {
            try {
                StreamWriter writer = new StreamWriter(sc.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            } catch (System.Exception e) {
                Debug.Log("Write error:" + e.Message);
            }

        }
    }
    private void broadcast(string data, serverClient c) {
        List<serverClient> sc = new List<serverClient> { c };
        broadcast(data, sc);
    }
    //Server Read
    private void onIncomingData(serverClient c, string data) {
        Debug.Log("SERVER: " + data);
        string[] aData = data.Split('|');
        switch (aData[0]) {
            case "CWHO":
                c.clientName = aData[1];
                c.isHost = (aData[2] == "0") ? false : true;
                broadcast("SCNN|" + c.clientName, clients);
                break;
            case "CMOV":
                broadcast("SMOV|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4], clients);
                break;
            case "CUPGR":
                broadcast("SUPGR|" + aData[1] + "|" + aData[2] + "|" + aData[3], clients);
                break;
            case "CSPELL":
                broadcast("SSPELL|" + aData[1] + "|" + aData[2] + "|" + aData[3], clients);
                break;
        }
    }
    private void startListening() {
        server.BeginAcceptTcpClient(acceptTcpClient, server);
    }
    private void acceptTcpClient(IAsyncResult ar) {
        TcpListener listener = (TcpListener)ar.AsyncState;

        string allUsers = "";
        foreach (serverClient _sc in clients) {
            allUsers += _sc.clientName + '|';
        }

        serverClient sc = new serverClient(listener.EndAcceptTcpClient(ar));
        clients.Add(sc);

        startListening();

        broadcast("SWHO|" + allUsers, clients[clients.Count - 1]);
    }
    private bool isConnected(TcpClient c) {
        try {
            if (c != null && c.Client != null && c.Client.Connected) {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                return true;
            } else
                return false;
        } catch {
            return false;
        }
    }
}
public class serverClient {
    public string clientName;
    public TcpClient tcp;
    public bool isHost;

    public serverClient(TcpClient tcp) {
        this.tcp = tcp;
    }
}
