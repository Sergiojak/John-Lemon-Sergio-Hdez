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

    //Para la velocidad de rotaci�n del personaje, al ser p�blica lo podemos cambiar desde el inspector.
    public float turnSpeed = 20f;

    //Variable Quaternion que almacena la rotaci�n (muy similar al Vector3)
    Quaternion m_Rotation = Quaternion.identity;

    //Declaramos el rigidbody para poder usarlo
    Rigidbody m_Rigidbody;

    void Start()
    {
        //Creo el acceso al componente Animator, la variable del Animator est� llamando al componente Animator 
        m_Animator = GetComponent<Animator>();
        //Lo mismo pero con Rigidbody, pillamos el acceso
        m_Rigidbody = GetComponent<Rigidbody>();

    }

    //Usamos FixedUpdate por temas de f�sicas
    void FixedUpdate()
    {
        //Creamos variables locales que recogen info del movimiento, en este caso del eje X.
        //Con el string (en ingl�s horizontal y vertical) Unity identifica el eje y si est�s tocando A o D o el Joystick izq o drcha
        float horizontal = Input.GetAxis("Horizontal");
        //Lo mismo para la vertical (que en realidad es el movimiento en Z profundidad, lo que visto cenitalmente)
        float vertical = Input.GetAxis("Vertical");

        //Con la variable Vector3 declarada creamos el movimiento., usando la funci�n Set de Vector3
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
        //En el string tenemos que poner EXACTAMENTE el nombre que pusimos en el animator del Unity, o sea el IsWalking, y luego el valor de la booleana (declarado encima por el isWalking)
        m_Animator.SetBool("IsWalking", isWalking);

        //Ahora para rotar al personaje:
        //La rotaci�n est� conformada por 3 elementos(XYZ) por lo que ser� un Vector3 que llamamos desiredforward (por ejemplo)
        //Ponemos transform.forward para que autom�ticamente pille la rotaci�n actual, luego ponemos m_Movement para que le indique hacia qu� valores va a rotar.
        //Luego usamos la funci�n de Vector3.RotateTowards y entre par�ntesis le ponemos los par�metros que nos pide. Sin embargo con esto a�n no hacemos nada.
        Vector3 desiredforward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        //Nos falta la variable quaternion que almacena la rotaci�n, una vez creada podemos configurarla
        //invocamos el m�todo LookRotation y crea una rotaci�n hacia el par�metro declarado anteriormente como desiredforward.
        m_Rotation = Quaternion.LookRotation(desiredforward);

        //Ahora aplicamos mov y rot al personaje, usando el rigidbody.

    }

    //Debido a que la animaci�n de movimiento del pj ya tiene desplazamiento, creamos una nueva funci�n.
    //Esta funci�n se llama en cada frame de la animaci�n ejecutada
    private void OnAnimatorMove()
    {
        //creamos una l�nea que nos permitir� mover al personaje
        //llamamos a la funci�n MovePosition del rigidbody y le decimos que su nueva posici�n ser� (posici�n actual + movimiento * magnituddeltadeanimaci�n)
        //sin embargo no podemos rotar a�n
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        //L�nea para rotar al personaje (ya hab�amos declarado previamente c�mo rotaba, asi que ponemos tan solo m_Rotation.
        m_Rigidbody.MoveRotation(m_Rotation);
    }

}
