using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSelectorController : MonoBehaviour
{
    private GameObject[] arrayAvatars;
    private int i; //Para guardar el índice que recorre el array
    
    void Start()
    {
        //El tamaño del array será un número igual al número de hijos
        //del gameobject AvatarSelector (a quien asignamos el script)
        arrayAvatars = new GameObject[transform.childCount];
        for (i = 0; i < transform.childCount; i++)
        {
            //relleno el array con los hijos
            arrayAvatars[i] = transform.GetChild(i).gameObject;
        }

        foreach (var avatar in arrayAvatars)
        {
            //Desactivo todos los avatars
            avatar.SetActive(false);
        }

        //Ahora activo por defecto, por ejemplo, el primer Avatar del array
        if (arrayAvatars[0] == true)
        {
            i = 0;
            arrayAvatars[0].SetActive(true); //Activamos el avatar actual
            PlayerPrefs.SetInt("AvatarSelected", i); //Y lo guardamos
        }
    }

    public void ChangeLeft()
    {
        arrayAvatars[i].SetActive(false); //Desactivamos el avatar actual
        i--; //Decrementamos el índice
        if (i < 0)
        {
            i = arrayAvatars.Length - 1; //Volvemos al último
        }
        arrayAvatars[i].SetActive(true); //Activamos el nuevo avatar
        PlayerPrefs.SetInt("AvatarSelected", i); //Y lo guardamos
    }
    
    public void ChangeRight()
    {
        arrayAvatars[i].SetActive(false); //Desactivamos el avatar actual
        i++; //Incremetamos el índice
        if (i > (arrayAvatars.Length - 1))
        {
            i = 0; //Volvemos al primero
        }
        arrayAvatars[i].SetActive(true); //Activamos el nuevo avatar
        PlayerPrefs.SetInt("AvatarSelected", i); //Y lo guardamos
    }
}
