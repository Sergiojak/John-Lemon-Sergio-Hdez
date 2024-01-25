using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Creamos una variable global que usaremos varias veces y la llamo como quiera, por ejemplo m_Movement
    //Ahora mismo es privada para este scritp porque no le he puesto public al principio
    Vector3 m_Movement;

    //vamos a necesitar el Animator de Unity para mover al personaje, asi que creamos una variable
    Animator m_Animator;

    void Start()
    {
        //Creo el acceso al componente Animator, la variable del Animator est� llamando al componente Animator 
        m_Animator = GetComponent<Animator>();


    }

    void Update()
    {
        //Creamos variables locales que recogen info del movimiento, en este caso del eje X.
        //Con el string (en ingl�s horizontal y vertical) Unity identifica el eje y si est�s tocando A o D o el Joystick izq o drcha
        float horizontal = Input.GetAxis("Horizontal");
        //Lo mismo para la vertical (que en realidad es el movimiento en Z profundidad, lo que visto cenitalmente)
        float vertical = Input.GetAxis("Vertical");

        //Con la variable Vector3 declarada creamos el movimiento
        m_Movement.Set(horizontal, 0f, vertical);
        //si lo dejamos tal cual, ir� m�s r�pido diagonalmente pq suma el eje horiz + vertic, por lo que tenemos que a�adir una funci�n Normalize
        m_Movement.Normalize();

        //ahora configuramos las animaciones del personaje.
        //Primero tenemos que identificar si se mueve o no para el idle o el walking (usando el bool)

        //declaramos variable booleana tieneInputhorizontal y usa la funci�n de la librer�a de matem�ticas (math)
        //y le decimos que si aproximadamente NO! tiene la posici�n horizontal 0f esto devuelve verdadero
        //Con la exclamaci�n detr�s de Mathf le decimos que NO, por lo que se leer�a como: NO (soy mayor de edad) 
        //Es decir, hasHorizontalInput es verdadero cuando NO es aproximadamente cero
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        //para que est� caminando se tiene que mover en horizontal o vertical. Usamos || para ese o
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        //Queremos que esta booleana actualice el animator IsWalking del Unity, para ello usamos la variable del Animator creada y lo configuramos:
        //En el string tenemos que poner EXACTAMENTE el nombre que pusimos en el animator del Unity, o sea el IsWalking
        m_Animator.SetBool("IsWalking", isWalking);
    }
}
