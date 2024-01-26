using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnding : MonoBehaviour
{
    //ahora creamos 1 variable para acceder a la duración del fade de la WinScreen (para que la pantalla se desvanezca en un período de tiempo específico )
    //y la referencia para acceder al GameObject jugador
    public float fadeDuration = 1f;
    public GameObject player;

    //crearemos una variable tipo bool para indicar que el jugador ha entrado en la WinZone
    bool m_IsPlayerAtExit;

    //Creamos variable del CanvasGroup para poder acceder al canvas y poder activarlo 
    public CanvasGroup exitBackgroundImageCanvasGroup;

    //tb creamos una tipo float para que el juego no termine antes de que el desvanecimiento haya ocurrido
    float m_Timer;

    //El tiempo que se permanece la imagen en pantalla
    public float displayImageDuration = 1f;

    void Start()
    {
        
    }

    //Creamos una función OnTriggerEnter para poder usar el trigger del collider de la WinZone
    private void OnTriggerEnter(Collider other)
    {
        //hacemos un if simple, si el Gameobject que toca el trigger es el Player, la variable booleana pasa a ser true.
        if(other.gameObject == player)
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
            EndLevel();

        }
    }
    void EndLevel()

    {
        //Usamos la variable del timer para indicar cuánto va a durar en el fin del juego 
        //y el componente.alpha del canvasgroup que tenemos, el cual durará el timer dividido en fadeDuration
        m_Timer += Time.deltaTime;
        exitBackgroundImageCanvasGroup.alpha = m_Timer / fadeDuration;

        //Ahora creamos un if para indicar que el juego no se cierre hasta que la imagen esté completa: if (m_Timer > fadeDuration)
        //pero quiero que permanezca un par de segundos en pantalla y no se cierre de manera inmediata 

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            //Con esto se cierra la aplicación (no pasará nada pq estamos desde el editor del Unity, no en un .exe)
            Application.Quit();
        }
    }
    //Importante no olvidarse de arrastrar todas las variables al inspector 
}
