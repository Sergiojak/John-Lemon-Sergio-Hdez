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

    //Para la velocidad de rotación del personaje, al ser pública lo podemos cambiar desde el inspector.
    public float turnSpeed = 20f;

    //Variable Quaternion que almacena la rotación (muy similar al Vector3)
    Quaternion m_Rotation = Quaternion.identity;

    //Declaramos el rigidbody para poder usarlo
    Rigidbody m_Rigidbody;

    void Start()
    {
        //Creo el acceso al componente Animator, la variable del Animator está llamando al componente Animator 
        m_Animator = GetComponent<Animator>();
        //Lo mismo pero con Rigidbody, pillamos el acceso
        m_Rigidbody = GetComponent<Rigidbody>();

    }

    //Usamos FixedUpdate por temas de físicas
    void FixedUpdate()
    {
        //Creamos variables locales que recogen info del movimiento, en este caso del eje X.
        //Con el string (en inglés horizontal y vertical) Unity identifica el eje y si estás tocando A o D o el Joystick izq o drcha
        float horizontal = Input.GetAxis("Horizontal");
        //Lo mismo para la vertical (que en realidad es el movimiento en Z profundidad, lo que visto cenitalmente)
        float vertical = Input.GetAxis("Vertical");

        //Con la variable Vector3 declarada creamos el movimiento., usando la función Set de Vector3
        m_Movement.Set(horizontal, 0f, vertical);
        //si lo dejamos tal cual, irá más rápido diagonalmente pq suma el eje horiz + vertic, por lo que tenemos que añadir una función Normalize
        m_Movement.Normalize();

        //ahora configuramos las animaciones del personaje.
        //Primero tenemos que identificar si se mueve o no para el idle o el walking (usando el bool)

        //declaramos variable booleana tieneInputhorizontal y usa la función de la librería de matemáticas (math)
        //y le decimos que si aproximadamente NO! tiene la posición horizontal 0f esto devuelve verdadero
        //Con la exclamación detrás de Mathf le decimos que NO, por lo que se leería como: NO (soy mayor de edad) 
        //Es decir, hasHorizontalInput es verdadero cuando NO es aproximadamente cero
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        //para que esté caminando se tiene que mover en horizontal o vertical. Usamos || para ese o
        bool isWalking = hasHorizontalInput || hasVerticalInput;

        //Queremos que esta booleana actualice el animator IsWalking del Unity, para ello usamos la variable del Animator creada y lo configuramos:
        //En el string tenemos que poner EXACTAMENTE el nombre que pusimos en el animator del Unity, o sea el IsWalking, y luego el valor de la booleana (declarado encima por el isWalking)
        m_Animator.SetBool("IsWalking", isWalking);

        //Ahora para rotar al personaje:
        //La rotación está conformada por 3 elementos(XYZ) por lo que será un Vector3 que llamamos desiredforward (por ejemplo)
        //Ponemos transform.forward para que automáticamente pille la rotación actual, luego ponemos m_Movement para que le indique hacia qué valores va a rotar.
        //Luego usamos la función de Vector3.RotateTowards y entre paréntesis le ponemos los parámetros que nos pide. Sin embargo con esto aún no hacemos nada.
        Vector3 desiredforward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        //Nos falta la variable quaternion que almacena la rotación, una vez creada podemos configurarla
        //invocamos el método LookRotation y crea una rotación hacia el parámetro declarado anteriormente como desiredforward.
        m_Rotation = Quaternion.LookRotation(desiredforward);

        //Ahora aplicamos mov y rot al personaje, usando el rigidbody.

    }

    //Debido a que la animación de movimiento del pj ya tiene desplazamiento, creamos una nueva función.
    //Esta función se llama en cada frame de la animación ejecutada
    private void OnAnimatorMove()
    {
        //creamos una línea que nos permitirá mover al personaje
        //llamamos a la función MovePosition del rigidbody y le decimos que su nueva posición será (posición actual + movimiento * magnituddeltadeanimación)
        //El deltaPosition.magnitude sincroniza el movimiento real con el desplazamiento de la animación 
        //sin embargo no podemos rotar aún´, para ello sirve la siguiente línea. 
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        //Línea para rotar al personaje (ya habíamos declarado previamente cómo rotaba, asi que ponemos tan solo m_Rotation)
        m_Rigidbody.MoveRotation(m_Rotation);
    }

}
