using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRoom : MonoBehaviour
{
    [SerializeField]
    private Text nameText; //Texto con el nombre de la sala
    [SerializeField]
    private Text sizeText; //Muestra el tamaño de la sala

    //Método al que llamaremos cuando el usuario pulse un botón del ScrollView, uniéndose el jugador a la sala indicada en el nameText
    public void JoinRoomOnClick()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby();
        }
        PhotonNetwork.JoinRoom(nameText.text);
        Debug.Log("Te has unido a la sala " + nameText.text);
    }
    
    //Se le llamará desde el método ListRoomInScrollView para cada botón que se instacia en el ScrollView
    public void SetRoom(string roomName, int roomCapacity, int playerCount)
    {
        nameText.text = roomName;
        sizeText.text = playerCount + "/" + roomCapacity;
    }
}
