using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float saludMaxima = 100f;
    private float saludActual;

    void Start()
    {
        saludActual = saludMaxima;
    }

    public void RecibirDa�o(float cantidad)
    {
        saludActual -= cantidad;
        Debug.Log("Jugador recibi� da�o: " + cantidad + " puntos. Salud actual: " + saludActual);

        if (saludActual <= 0)
        {
            Morir();
        }
    }

    public void Curar(float cantidad)
    {
        saludActual += cantidad;
        if (saludActual > saludMaxima)
        {
            saludActual = saludMaxima;
        }
        Debug.Log("Jugador se cur�: " + cantidad + " puntos. Salud actual: " + saludActual);
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");

    }
}