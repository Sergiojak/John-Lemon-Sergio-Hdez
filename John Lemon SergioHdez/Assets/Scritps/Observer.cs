using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    //Este es el Script para que al pasar por el cono de visi�n del enemigo, salte la pantalla de derrota.
    //el collider va a necesitar saber qui�n es el jugador (m�s bien su posici�n con Transform) para poder identificarlo
    //y si el jugador est� dentro del rango de visi�n
    public Transform player;
    bool m_IsPlayerInRange;

    //y una vez se haga todo lo del trigger y el rayo, debe saltar la LoseScreen,
    //por lo que llamamos a la clase (scritp) del GameEnding creado anteriormente
    //(a la cual hace falta a�adirle lo mismo que salte el winscreen pero con la losescreen)
    public GameEnding gameEnding;

    void Start()
    {
        
    }

    //Creamos una funci�n OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //Hacemos un if, si el jugador entra en la zona de visi�n, est�enrango = true (cambia la booleana)
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }

    }
    //Creamos una funci�n OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        //Hacemos un if, si el jugador sale de la zona de visi�n, est�enrango = false (cambia la booleana)
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
        //Con este if generamos el rayo que choca con una pared o con el jugador
        if(m_IsPlayerInRange)
        {
            //con las siguientes 2 l�neas vamos a crear un rayo (la primera da la localizaci�on desde la g�rgola hasta el player y la segunda crea el rayo en s�)
            Vector3 direction = player.position - transform.position + Vector3.up;
            Ray ray = new Ray(transform.position, direction);

            //Ahora necesitamos almacenar la informaci�n del rayo (que nos avise que ha tenido colisi�n) para ello usamos ua variable 
            RaycastHit raycastHit;

            if (Physics.Raycast(ray, out raycastHit))
            {
                //est� viendo algo (sea pared o jugador) necesitamos comprobar que no haya nada en medio y sea el jugador.
              if (raycastHit.collider.transform == player)
                {
                    //est� viendo al jugador
                    //llamamos a la funci�n CaughtPlayer del script/clase gameEnding, (que vuelve verdadero que han pillado al personaje)
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }



}
