using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayersDamageController : MonoBehaviour
{
    public Image barraDeVida;
    [SerializeField]
    private float health; //Para guardar los puntos de vida del player
    [SerializeField]
    private float vidaActual;
    

    [PunRPC] //Este método podrá ser llamado por todos los jugadores de la misma sala
    public void Damage(float damage)
    {
        vidaActual = vidaActual - damage; //Restamos puntos de vida al recibir un impacto de bala
        barraDeVida.fillAmount = vidaActual / health;
        Debug.Log("Vida: " + health);
        if (vidaActual < 0)
        {
            GetComponent<ParticleSystem>().Play();
            vidaActual = 0;
            Debug.Log("Has muerto");
        }
    }
}
