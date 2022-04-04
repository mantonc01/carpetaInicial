using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab; //Para asignarle el prefab que tiene el PhotonView
    [SerializeField]
    private GameObject[] destroyablesPrefab; //Para asignarle los prefabs de las cajas
    
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (playerPrefab != null)
            {
                int randomPosition = Random.Range(20, 29); //Para que el prefab aparezca en un lugar de la escena controlado
                Vector3 position = new Vector3(randomPosition, 0, randomPosition);
                PhotonNetwork.Instantiate(playerPrefab.name, position, Quaternion.identity);
            }
            
            if (PhotonNetwork.IsMasterClient)
            {
                for (int i = 0; i < destroyablesPrefab.Length; i++)
                {
                    int randomPosition = Random.Range(20, 29); //Para que el prefab aparezca en un lugar de la escena controlado
                    Vector3 position = new Vector3(randomPosition, 0, randomPosition);
                    PhotonNetwork.Instantiate(destroyablesPrefab[i].name, position, Quaternion.identity);
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
    }

    public override void OnJoinedRoom()
    {
        
    }
}
