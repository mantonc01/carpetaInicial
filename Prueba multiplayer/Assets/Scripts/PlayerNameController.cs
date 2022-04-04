using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using WebSocketSharp;

public class PlayerNameController : MonoBehaviour
{
    //Método al que le llega como parámetro el nombre del jugador
    //Este método será llamado al introducir texto en el InputField
    public void setPlayerName(string playerName)
    {
        if (playerName.IsNullOrEmpty() == false)
        {
            //Efectuaremos la conexion al servidor de Photon
            PhotonNetwork.NickName = playerName;
        }
    }
}
