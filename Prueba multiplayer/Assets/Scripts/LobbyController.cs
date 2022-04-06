using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviourPunCallbacks
{
    public Text textRoomName;
    public Text textRoomCapacity;
    public Text textNamePlayer;
    private string _roomName; //Para guardar el nombre de la sala que introduzca el usuario
    private int _roomCapacity; //Para guardar la capacidad de la sala que introduzca el usuario
    [SerializeField]
    private GameObject _panelLobby; //Panel para mostrar/ocultar el Lobby
    [SerializeField]
    private GameObject _panelUser; //Panel para mostrar el menú principal
    [SerializeField]
    private GameObject _panelRoom; //Panel para mostrar el menú de jugadores en sala
    public Transform Content; //Contenedor para los botones con las salas disponibles
    public GameObject prefabRoomInList; //Prefab de cada botó en la lista de salas del ScrollView
    private Dictionary<string, RoomInfo> cachedRoomList; //Para volcar la roomList de Photon

    public void Awake()
    {
        cachedRoomList = new Dictionary<string, RoomInfo>(); //Inicializamos nuestra cachedRoomList
    }

    // Start is called before the first frame update
    void Start()
    {
        //PhotonNetwork.JoinLobby(); //Entramos en la sala de espera (y se llama automáticamente al OnRoomListUpdate)
        //textNamePlayer.text = PhotonNetwork.NickName; //Establecemos el valor del campo TextNamePlayer
    }

    public void SetRoomName()
    {//Guardamos el nombre de la sala que ha introducido el usuario
        _roomName = textRoomName.text;
    }

    public void SetRoomCapity()
    {//Guardamos la capacidad de la sala que ha introducido el usuario
        _roomCapacity = int.Parse(textRoomCapacity.text);
    }

    public void CreateRoom()
    {
        SetRoomName();
        SetRoomCapity();
        Debug.Log("Creating a new room '" + _roomName + "' ......");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte) _roomCapacity;
        roomOptions.IsOpen = true;
        if (string.IsNullOrEmpty(_roomName) == false && _roomCapacity != 0)
        {
            PhotonNetwork.CreateRoom(_roomName, roomOptions);
            Debug.Log("You have create the room");
        }
    }
//----------------------------------
    //Cada vez que hay un JoinLobby, o un cambio en la lista de salas (por ejemplo, un usuario que entre a que salga de una sala) se llama a este método automáticamente
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        DeleteScrollViewRooms(); //Primero borro toda la lista de salas (prefab de botones) del ScrollViewRooms del panel
        /*foreach (RoomInfo room in roomList) //Recorro la lista de salas que está en la roomList de Photon
        {
            ListRoomInScrollView(room); //Lista la sala en el ScrollViewRoms del panel
        }*/
        UpdateCachedRoomList(roomList); //Volcamos la roomList a nuestra cachedRoomList
        UpdateRoomListView(); //Actualizamos la lista de hijos (prefab de botones) de Content
    }
    
    //Método para volcar la roomList de PhotonView en una lista nuestra llamada cachedRoomList
    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo room in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(room.Name))
                {
                    cachedRoomList.Remove(room.Name); //La borramos de nuestra cachedRoomList
                }

                continue; //Para que itere de nuevo el foreach sin ejecutar lo que viene debajo
            }
            
            // Update cached room info (para actualizar el número de jugadores)
            if (cachedRoomList.ContainsKey(room.Name)) //Si la sala ya está en nuestra cachedRoomList
            {
                cachedRoomList[room.Name] = room; //Sobreescribimos esta sala en nuestra lista cachedRoomList
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(room.Name, room); //Añadimos esta sala a nuestra lista cachedRoomList
            }
        }
    }
    
    private void UpdateRoomListView()
    {
        foreach (RoomInfo room in cachedRoomList.Values)
        {
            GameObject prefabButtonRoom = Instantiate(prefabRoomInList, Content);
            //prefabButtonRoom.transform.SetParent(Content.transform);
            //prefabButtonRoom.transform.localScale = Vector3.one;
            prefabButtonRoom.GetComponent<ButtonRoom>().SetRoom(room.Name, room.MaxPlayers, room.PlayerCount);

            //roomListEntries.Add(room.Name, prefabButtonRoom);
        }
    }
    
    //--------------------------------------

    private void DeleteScrollViewRooms()
    {
        for (int i = Content.childCount-1; i >= 0; i--)
        {
            Destroy(Content.GetChild(i).gameObject);
        }
    }

    void ListRoomInScrollView(RoomInfo room) //Cada elemento de la lista del ScrollView
    {
        if (room.IsOpen && room.IsVisible)
        {
            GameObject prefabButtonRoom = Instantiate(prefabRoomInList, Content); //Instancia el prefab del botón en el contenedor del ScrollView
            ButtonRoom buttonRoom = prefabButtonRoom.GetComponent<ButtonRoom>(); //Creo un objeto a partir de su clase ButtonRoom
            buttonRoom.SetRoom(room.Name, room.MaxPlayers, room.PlayerCount); //Establezco los atributos del botón
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("This room name already exist.");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("You have joined the '" + _roomName + "' room.");
        _panelRoom.SetActive(true); //Activamos el panel Room
        _panelLobby.SetActive(false); //Desactivamos el panel Lobby
        
        _panelRoom.GetComponent<RoomController>().DeleteAllPlayersInList();
        _panelRoom.GetComponent<RoomController>().FillPlayersList();
        
        GameObject TextNamePlayer = _panelRoom.transform.Find("TextNamePlayer").gameObject;
        TextNamePlayer.GetComponent<Text>().text = PhotonNetwork.NickName;
        GameObject TextRoomName = _panelRoom.transform.Find("TextRoomName").gameObject;
        TextRoomName.GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;

        //Sólo el jugador que crea la room puede iniciar la partida, por lo que sólo el Master (el que crea la room) tendrá el botón de 'Start Game' activo en el PanelRoom
        if (PhotonNetwork.IsMasterClient)
        {
            _panelRoom.GetComponent<RoomController>().buttonStartGame.SetActive(true);
        }
        else
        {
            _panelRoom.GetComponent<RoomController>().buttonStartGame.SetActive(false);
        }
    }

    public void ExitPanelLobby()
    {
        _panelUser.SetActive(true);
        _panelLobby.SetActive(false);
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.Disconnect();
        Debug.Log("Exit Looby");
    }
}
