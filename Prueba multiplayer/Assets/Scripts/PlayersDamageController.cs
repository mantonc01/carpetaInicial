using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayersDamageController : MonoBehaviour
{
    [SerializeField]
    private float health; //Para guardar los puntos de vida del player
    [SerializeField]
    private Text txtHealth; //Referencia al marcador de puntos del Canvas

    [PunRPC] //Este método podrá ser llamado por todos los jugadores de la misma sala
    public void Damage(float damage)
    {
        health = health - damage; //Restamos puntos de vida al recibir un impacto de bala
        txtHealth.text = health.ToString(); //Actualizamos el marcador de puntos del Canvas
        Debug.Log("Vida: " + health);
        if (health < 0)
        {
            health = 0;
            Debug.Log("Has muerto");
        }
    }
}
