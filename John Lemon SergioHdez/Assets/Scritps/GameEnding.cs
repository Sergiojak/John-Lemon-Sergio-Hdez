using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    //ahora creamos 1 variable para acceder a la duraci�n del fade de la WinScreen (para que la pantalla se desvanezca en un per�odo de tiempo espec�fico )
    //y la referencia para acceder al GameObject jugador
    public float fadeDuration = 1f;
    public GameObject player;

    //crearemos una variable tipo bool para indicar que el jugador ha entrado en la WinZone
    bool m_IsPlayerAtExit;

    //Creamos variable del CanvasGroup para poder acceder al canvas y poder activarlo, y lo mismo para la derrota (no olvidarse de a�adirlo en el hierarchy)
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public CanvasGroup caughtBackgroundImageCanvas;

    //tb creamos una tipo float para que el juego no termine antes de que el desvanecimiento haya ocurrido
    float m_Timer;

    //El tiempo que se permanece la imagen en pantalla
    public float displayImageDuration = 1f;

    //ahora para si es capturado
    bool m_IsPlayerCaught;

    //Creamos una funci�n OnTriggerEnter para poder usar el trigger del collider de la WinZone
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
        //Si IsPlayerAtExit es true, llamamos a la funci�n EndLevel
        if (m_IsPlayerAtExit == true)
        {
            //este para que sea de la victoria le ponemos en los par�ntesis exitbackgr
            EndLevel(exitBackgroundImageCanvasGroup, false);
            //Hasta aqu� solo funciona para la victoria, falta para la derrota, por lo que a�adimos otro if
            //Le decimos que si no es verdadero, verifique si isPlayerCaught es verdadero
        }
        else if (m_IsPlayerCaught == true)
        {
            //este para que sea de la derrota le ponemos en los par�ntesis caughtbackgr
            EndLevel(caughtBackgroundImageCanvas, true);
        }

        /* Otra manera de hacerlo m�s ordenada y compacta es as�, usando un "or" (las dos barritas || as�) pero el tutorial lo pide de otra manera
        if (m_IsPlayerAtExit|| m_IsPlayerCaught)
        {
            EndLevel();
        }*/

    }
    //Para que use la  imagen del canvas de victoria o derrota dependiendo de lo que suceda, necesitamos a�adirle a la funci�n CanvasGroup imageCanvasGroup
    //tb le creamos un bool para que no se reinicie el juego si ganamos pero s� cuando perdemos (vamos a las funciones EndLevel de los ifs de encima y le ponemos a uno false y al otro true)
    void EndLevel(CanvasGroup imageCanvasGroup, bool doRestart)

    {
        //usamos la variable del timer para que incremente por Time.deltatime, para que vaya suave cambiando con los FPSs
        //y el componente.alpha del canvasgroup que tenemos, el cual durar� el timer dividido en fadeDuration, es decir,
        //har� la transici�n del alpha a lo largo de lo que dure el fade duration (de forma suave gracias a que timer += Timedeltatime)
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        //Ahora creamos un if para indicar que el juego no se cierre hasta que la imagen est� completa: if (m_Timer > fadeDuration)
        //pero quiero que permanezca un par de segundos en pantalla y no se cierre de manera inmediata 

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            //Con esto se cierra la aplicaci�n (no pasar� nada pq estamos desde el editor del Unity, no en un .exe)
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
    //tb creamos una funci�n p�blica para que el m_IsPlayerCaught se vuelva true si entra en colisi�n con los rayos/collider del enemigo, por lo que debemos hacerlo en el script del observer
    public void CaughtPlayer()
    {
        m_IsPlayerCaught = true;
    }


    //Importante no olvidarse de arrastrar todas las variables al inspector (porque si no el script no s�be qui�n es player ni el tiempo y tal) 
}
