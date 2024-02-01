using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    //EXAMEN JOHN LEMON 
    //Temporizador para detectar al personaje
    //M�ximo para ser detectado 2 segundos
    public float m_LimiteDeteccion = 2f;
    //Temporizador que va aumentando cada frame (igual que con el timer del GameEnding y su float de desvanecimiento de imagen)
    //Usar time.Deltatime; con el += para que aumente por frame en el Update
    float m_Detectando;

    //Audio Alerta
    //Usamos la componente
    public AudioSource alertaTindeck;
    //para que no suene constantemente usamos la bool
    bool m_AudioAlertaNecesario;

    public GameObject ExclamacionAlerta;


    void Start()
    {
        //No hace falta usar el GetComponent del AudioSource porque ya lo a�adimos al prefab desde el inspector
        m_AudioAlertaNecesario = false;
        m_Detectando = 0f;

    }

    //Creamos una funci�n OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //Hacemos un if, si el jugador entra en la zona de visi�n, est�enrango = true (cambia la booleana)
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;

            //el jugador entr� en el trigger, ahora que se active el audio
            m_AudioAlertaNecesario = true;
            if(m_AudioAlertaNecesario == true)
            {
                alertaTindeck.Play();
                Debug.Log("Audio de Alerta Tindeck Sonando");
            }
        }
    }
    //Creamos una funci�n OnTriggerExit
    private void OnTriggerExit(Collider other)
    {
        //Hacemos un if, si el jugador sale de la zona de visi�n, est�enrango = false (cambia la booleana)
        if(other.transform == player)
        {
            m_IsPlayerInRange = false;
            //salimos del trigger, no necesitamos el audio
            m_AudioAlertaNecesario = false;

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
                    //El tiempo empieza a contar
                    m_Detectando += Time.deltaTime;
                    Debug.Log("Te est�n viendo " + m_Detectando);
                    Debug.Log("Quedan " + (m_Detectando - m_LimiteDeteccion) + " segundos para ser detectado" );
                    //Llama a la funci�n que acabar� la partida si el jugador es detectado por 2 segundos
                    DetectandoJugador();
                    Alerta();
              }
            }
        }
        else
        {
            //Si NO est� el jugador en rango del rayo (else), el temporizador estar� en CERO
            m_Detectando = 0f;
            ExclamacionAlerta.SetActive(false);

        }
    }
    void DetectandoJugador()
    {
        //Si el tiempo que ha empezado a contar supera el l�mite puesto, se har� lo siguiente
        if (m_Detectando > m_LimiteDeteccion)
        {
            //llamamos a la funci�n CaughtPlayer del script/clase gameEnding, (que vuelve verdadero que han pillado al personaje)
            //Con esto se acaba la partida
            gameEnding.CaughtPlayer();
            //aparece pantalla de derrota
            Debug.Log("Te han pillado! Fin de la partida.");
        }
    }
    //Funci�n para que se active el canvas y muestre la exclamaci�n
    void Alerta()
    {
        ExclamacionAlerta.SetActive(true);
        Debug.Log("Aparece exclamacion");
    }
}
