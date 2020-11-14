using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
//using System.Net.Sockets;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; set; }
    public GameObject mainMenu;
    public GameObject serverMenu;
    public GameObject connectMenu;
    public GameObject serverPrefab;
    public GameObject clientPrefab;
    public InputField nameInput;
    public Text IPText;
    private void Start() {
        Instance = this;
        mainMenu.SetActive(true);
        DontDestroyOnLoad(gameObject);
    }
    public void connectButton() {
        mainMenu.SetActive(false);
        connectMenu.SetActive(true);
    }
    public void hostButton() {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        IPText.text = "Your IP Adress is: " + host.AddressList[1].ToString();
        try {
            Server s = Instantiate(serverPrefab).GetComponent<Server>();
            s.init();
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            c.isHost = true;
            if (c.clientName == "")
                c.clientName = "Unknown Host Player";
            c.connectToServer("127.0.0.1", 6321);
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }

        mainMenu.SetActive(false);
        serverMenu.SetActive(true);
    }
    public void connectToServerButton() {
        string hostAddress = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (hostAddress == "")
            hostAddress = "127.0.0.1";
        try {
            Client c = Instantiate(clientPrefab).GetComponent<Client>();
            c.clientName = nameInput.text;
            if (c.clientName == "")
                c.clientName = "Unknown Player";
            c.connectToServer(hostAddress, 6321);
            connectMenu.SetActive(false);
        } catch (System.Exception e) {
            Debug.Log(e.Message);
        }
    }
    public void backButton() {
        serverMenu.SetActive(false);
        connectMenu.SetActive(false);
        mainMenu.SetActive(true);

        Server s = FindObjectOfType<Server>();
        if (s != null)
            Destroy(s.gameObject);
        Client c = FindObjectOfType<Client>();
        if (c != null)
            Destroy(c.gameObject);
    }
    public void startGame() {
        SceneManager.LoadScene("Game");
    }
}
