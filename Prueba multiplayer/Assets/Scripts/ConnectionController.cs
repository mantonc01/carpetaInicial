using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionController : MonoBehaviourPunCallbacks
{
    public GameObject panelUser;
    public GameObject panelConnect;
    public GameObject panelLobby;
    public GameObject panelRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.ConnectUsingSettings();
        panelUser.SetActive(true);
        RandomName();
        panelConnect.SetActive(false);
        panelLobby.SetActive(false);
        panelRoom.SetActive(false);
    }

    void RandomName()
    {
        InputField inputFieldNickname = panelUser.GetComponentInChildren<InputField>();
        inputFieldNickname.text = "Player " + Random.Range(1, 100);
    }
    
    public void PhotonServerConnect()
    {
        //Le llamamos desde el botón Enter del panelUser
        if (PhotonNetwork.IsConnected == false && string.IsNullOrEmpty(PhotonNetwork.NickName) == false)
        {
            PhotonNetwork.ConnectUsingSettings();
            
            panelUser.SetActive(false);
            panelConnect.SetActive(true);
        }
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Estamos contectados a Internet");
    }
    
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        
        //Indicamos que todos los jugadores de la sala usarán la misma que el jugador master (el que crea la sala) o Master Client
        PhotonNetwork.AutomaticallySyncScene = true;
        
        panelConnect.SetActive(false);
        panelLobby.SetActive(true);

        GameObject TextNamePlayer = panelLobby.transform.Find("TextNamePlayer").gameObject;
        TextNamePlayer.GetComponent<Text>().text = PhotonNetwork.NickName;
        
        PhotonNetwork.JoinLobby(); //Entramos en la sala de espera (y se llama automáticamente al OnRoomListUpdate)
        
        Debug.Log("Estamos contectados al servidor Photon");
        Debug.Log("Bienvenido " + PhotonNetwork.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Nos hemos desconectados del servidor Photon");
    }

    /*public void JoinRandomRoom()
    {
        //Creo una Sala o Room al azar para unirse
        PhotonNetwork.JoinRandomRoom();
    }*/
    
    //Si no se puede unir a una sala al azar se ejecuta el siguiente método:
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("No se puede unir a la sala. " + message);
        //CreateRoomAndJoin();
    }

    /*private void CreateRoomAndJoin()
    {
        string nameUser = PhotonNetwork.NickName; //Obtenemos el nombre del usuario
        Debug.Log("UserName: " + nameUser);
        
        //Creamos una sala con el nombre de usuario y un número al azar
        //string roomName = "Room: " + nameUser + Random.Range(1, 1000);
        string roomName = "Test Room";
        
        //Opciones de la sala
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;
        roomOptions.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
        Debug.Log("You joined the room: " + roomName);
    }*/

    /*public override void OnJoinedRoom()
    {
        Debug.Log("You joined the room: " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Number of players: " + PhotonNetwork.CurrentRoom.PlayerCount);

        Button buttonCreateRoom = panelRoom.GetComponentInChildren<Button>();
        buttonCreateRoom.enabled = false; //Desactivamos el botón "Create / Enter a Room"

        Transform textInfoRoom = panelRoom.transform.Find("TextInfoRoom");
        textInfoRoom.GetComponent<Text>().text = "You have joined the " + PhotonNetwork.CurrentRoom.Name +
                                                 "\nNumer of players in this room: " +
                                                 PhotonNetwork.CurrentRoom.PlayerCount;
    }*/
}
