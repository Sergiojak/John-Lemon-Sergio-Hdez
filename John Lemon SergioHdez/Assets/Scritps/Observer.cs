using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    //el collider va a necesitar saber qui�n es  el jugador para poder identificarlo
    //y si el jugador est� dentro del rango de visi�n
    public Transform player;
    bool m_IsPlayerInRange;
    void Start()
    {
        
    }

    //Creamos una funci�n OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //Hacemos un if, si el jugador entra en la zona de visi�n, est�enrango = true
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }

    }
    //Creamos una funci�n OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        //Hacemos un if, si el jugador sale de la zona de visi�n, est�enrango = false
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
        }

    }
    //Sin embargo con lo que tenemos ahora, si el jugador activa el trigger se acabar�a la partida, pero no deber�a ser as� porque si hay una pared por delante
    //el trigger se activar�a pero la g�rgola no le est� viendo realmente
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
                //Est� viendo algo
                if(raycastHit.collider.transform == player)
                {
                    //Est� viendo al jugador
                }
            }
        }
    }

}
