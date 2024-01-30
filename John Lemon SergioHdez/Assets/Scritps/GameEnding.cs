using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    //ahora creamos 1 variable para acceder a la duración del fade de la WinScreen (para que la pantalla se desvanezca en un período de tiempo específico )
    //y la referencia para acceder al GameObject jugador
    public float fadeDuration = 1f;
    public GameObject player;

    //crearemos una variable tipo bool para indicar que el jugador ha entrado en la WinZone
    bool m_IsPlayerAtExit;

    //Creamos variable del CanvasGroup para poder acceder al canvas y poder activarlo, y lo mismo para la derrota (no olvidarse de añadirlo en el hierarchy)
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvas;

    //tb creamos una tipo float para que el juego no termine antes de que el desvanecimiento haya ocurrido
    float m_Timer;

    //El tiempo que se permanece la imagen en pantalla
    public float displayImageDuration = 1f;

    //ahora para si es capturado
    bool m_IsPlayerCaught;

    //Para que funcionen los sonidos de cuando ganas y te pillan necesitamos crear 3 variables
    //Una para el sonido de win, otra para el sonido de lose y otra bool para saver si se ha reproducido el sonido o no
    public AudioSource exitSource;
    public AudioSource caughtSource;
    bool m_HasAudioPlayed;

    //Creamos una función OnTriggerEnter para poder usar el trigger del collider de la WinZone
    private void OnTriggerEnter(Collider other)
    {
        //hacemos un if simple, si el Gameobject que toca el trigger es el Player, la variable booleana pasa a ser true.
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }

    }
    void Update()
    {
        //una vez que en el trigger la variable se haya vuelto true, creamos en el Update el siguiente if
        //Si IsPlayerAtExit es true, llamamos a la función EndLevel
        if (m_IsPlayerAtExit == true)
        {
            //este para que sea de la victoria le ponemos en los paréntesis exitbackgr
            EndLevel(exitBackgroundImageCanvasGroup, false, exitSource);
            //Hasta aquí solo funciona para la victoria, falta para la derrota, por lo que añadimos otro if
            //Le decimos que si no es verdadero, verifique si isPlayerCaught es verdadero
        }
        else if (m_IsPlayerCaught == true)
        {
            //este para que sea de la derrota le ponemos en los paréntesis caughtbackgr
            EndLevel(caughtBackgroundImageCanvas, true, caughtSource);
        }

        /* Otra manera de hacerlo más ordenada y compacta es así, usando un "or" (las dos barritas || así) pero el tutorial lo pide de otra manera
        if (m_IsPlayerAtExit|| m_IsPlayerCaught)
        {
            EndLevel();
        }*/

    }
    //Para que use la  imagen del canvas de victoria o derrota dependiendo de lo que suceda, necesitamos añadirle a la función CanvasGroup imageCanvasGroup
    //tb le creamos un bool para que no se reinicie el juego si ganamos pero sí cuando perdemos (vamos a las funciones EndLevel de los ifs de encima y le ponemos a uno false y al otro true)
    //por último le añadimos el audioSource (hay que cambiarlo en todo tb y decirle cuál es cuál)
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)

    {
        //Este if nos confirma que el sonido no ha sido reproducido
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }


        //usamos la variable del timer para que incremente por Time.deltatime, para que vaya suave cambiando con los FPSs
        //y el componente.alpha del canvasgroup que tenemos, el cual durará el timer dividido en fadeDuration, es decir,
        //hará la transición del alpha a lo largo de lo que dure el fade duration (de forma suave gracias a que timer += Timedeltatime)
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        //Ahora creamos un if para indicar que el juego no se cierre hasta que la imagen esté completa: if (m_Timer > fadeDuration)
        //pero quiero que permanezca un par de segundos en pantalla y no se cierre de manera inmediata 

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            //Con esto se cierra la aplicación (no pasará nada pq estamos desde el editor del Unity, no en un .exe)
            if (doRestart)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                Application.Quit();
            }
        }
    }
    //tb creamos una función pública para que el m_IsPlayerCaught se vuelva true si entra en colisión con los rayos/collider del enemigo, por lo que debemos hacerlo en el script del observer
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }


    //Importante no olvidarse de arrastrar todas las variables al inspector (porque si no el script no sábe quién es player ni el tiempo y tal) 
}
