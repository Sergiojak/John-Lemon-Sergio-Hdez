using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    //el collider va a necesitar saber quién es  el jugador para poder identificarlo
    //y si el jugador está dentro del rango de visión
    public Transform player;
    bool m_IsPlayerInRange;
    void Start()
    {
        
    }

    //Creamos una función OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //Hacemos un if, si el jugador entra en la zona de visión, estáenrango = true
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }

    }
    //Creamos una función OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        //Hacemos un if, si el jugador sale de la zona de visión, estáenrango = false
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
        }

    }
    //Sin embargo con lo que tenemos ahora, si el jugador activa el trigger se acabaría la partida, pero no debería ser así porque si hay una pared por delante
    //el trigger se activaría pero la gárgola no le está viendo realmente
    //por ello usaremos un rayo
    void Update()
    {
        //Con este if
        if(m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);
            if (Physics.Raycast(ray, out raycastHit))
            {
                //Está viendo algo
                if(raycastHit.collider.transform == player)
                {
                    //Está viendo al jugador
                }
            }
        }
    }

}
