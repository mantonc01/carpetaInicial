using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class FireController : MonoBehaviourPunCallbacks
{
    public float fireDistance; //Longuitud del Raycast
    public float pointsDamage; //Daño de cada disparo
    private Animator animator;
    private AudioSource damageSource;
    public AudioClip damageSound;
    
    // Update is called once per frame
    void Update()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 6;

        RaycastHit hit; //Aquí se guardará la referenciaa al objeto con el que impacta el Raycast


        if (Input.GetKeyDown(KeyCode.Mouse0)&&photonView.IsMine)
        {
            transform.Rotate(transform.rotation.x,transform.rotation.y+60,transform.rotation.z);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0)&&photonView.IsMine)
        {
            transform.Rotate(transform.rotation.x,transform.rotation.y-60,transform.rotation.z);
        }
        
        if (Input.GetKey(KeyCode.Mouse0) && photonView.IsMine)
        {
            
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position + new Vector3(0, .25f, 0), transform.TransformDirection(Vector3.forward+ new Vector3(-1.73f,0,0)), out hit, fireDistance, layerMask))
            {
                string hitTag = hit.collider.tag;
                if (hitTag != "Player")
                {
                    Debug.Log("Le has dado a " + hit.collider.gameObject.name);
                    //PhotonNetwork.Destroy(hit.collider.gameObject);
                    hit.transform.GetComponent<ParticleSystem>().Play();
                    StartCoroutine(DestruirObjeto(hit.transform.gameObject));
                }
                else
                {
                    Debug.Log("Le has dado a " + hit.collider.gameObject.name);
                    hit.collider.GetComponent<PhotonView>().RPC("Damage", RpcTarget.AllBufferedViaServer, pointsDamage);
                    animator = hit.collider.gameObject.GetComponent<Animator>();
                    animator.SetTrigger("isDamage");
                    animator.CrossFadeInFixedTime("Damage", 0.3f);
                    damageSource = hit.collider.gameObject.GetComponent<AudioSource>();
                    damageSource.clip = damageSound;
                    damageSource.Play();
                }
                
            }
            
        }
    }

    [PunRPC]
    public void DestroyTarget(GameObject destroyable)
    {
        Destroy(destroyable);
    }

    IEnumerator DestruirObjeto(GameObject destroyable)
    {
        yield return new WaitForSeconds(2f);
        DestroyTarget(destroyable);
        
    }
    
}
