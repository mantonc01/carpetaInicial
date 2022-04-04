using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviourPunCallbacks
{
    public Text textRoomName;
    public Text textPlayerName;
    [SerializeField]
    private GameObject _panelLobby; //Panel para mostrar el Lobby
    [SerializeField]
    private GameObject _panelRoom; //Panel para ocultar el menú de jugadores en sala
    public GameObject buttonStartGame; //Botón Start Game, sólo está activo para el Master
    public Transform content; //Contenedor donde guardaremos los prefabs
    public GameObject prefabPlayerInList; //Prefab para mostrar cada jugador en la lista (el prefab que hemos creado)
    [SerializeField]
    private int indexMultiPlayerScene;

    // Start is called before the first frame update
    void Start()
    {
        //Establecemos el valor del campo TextNamePlayer con el nombre del jugador
        //textPlayerName.text = PhotonNetwork.NickName;
        //Establecemos el valor del campo TextRoomName con el nombre de esta sala
        //textRoomName.text = PhotonNetwork.CurrentRoom.Name;
    }

    public void ExitPanelRoom() //Método asociado al botón 'Exit this room'
    {
        PhotonNetwork.LeaveRoom(); //Abandono la sala
        //PhotonNetwork.LeaveLobby(); //Abandono el lobby
        //StartCoroutine(ReJoinLobby());
        
        PhotonNetwork.JoinLobby(); //Me vuelvo a unir al lobby y así se llama automáaticamente al método OnRoomListUpdate
        _panelRoom.SetActive(false);
        _panelLobby.SetActive(true);
    }

    public void FillPlayersList()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(prefabPlayerInList, content);
            Text tempText = tempListing.transform.GetChild(0).GetComponent<Text>();
            tempText.text = player.NickName;
        }
    }
    
    public void DeleteAllPlayersInList()
    {
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        DeleteAllPlayersInList();
        FillPlayersList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        DeleteAllPlayersInList();
        FillPlayersList();
    }

    public void StartGameOnClick()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            //Como se ha puesto la sincronización automática de escena, el master notifica automáticamente a los demás jugadores antes de cargar la escena
            PhotonNetwork.LoadLevel(indexMultiPlayerScene);
        }
    }

    /*IEnumerator ReJoinLobby()
    {
        yield return new WaitForSeconds(2); //Espero 2 segundos
        PhotonNetwork.JoinLobby(); //Me vuelvo a unir al lobby y así se llama automáaticamente al método OnRoomListUpdate
        _panelRoom.SetActive(false);
        _panelLobby.SetActive(true);
    }*/
}
