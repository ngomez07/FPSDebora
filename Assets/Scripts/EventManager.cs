using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; // necesito esta clase para usar los eventos

[Serializable] // de una te importa el System en la libreria
public class Int2Event : UnityEvent<int,int>
{
    // creamos una clase con el tipo de evento que recibe dos de tipo entero
}

public class EventManager : MonoBehaviour
{
    // este evant manager se hace para que los otros scripts funcionen independientemente, asi no hay que enlazar, funciona usando un event system, BUENA PRACTICA

    // decir que es un singleton, le dice a unity que solo puedo instanciar la funcion una vez nada mas
    // los eventos los pueden llamar desde otros scripts, asi no hay que enlazar mas cosas!

    #region Singleton
    public static EventManager current; // creamos una variable igual que nuestra clase

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }else if (current != null)
        {
            Destroy(this);
        }
    }

    #endregion

    // creo evento que se recibe el numero total de balas y el numero actual de balas, el lo que hace es que recibe los valores y los convierte en eventos.
    public Int2Event updateBulletsEvent = new Int2Event();

    // como es normal y no depende de parametros, lo puedo hacer asi
    public UnityEvent NewGunEvent = new UnityEvent();
}
