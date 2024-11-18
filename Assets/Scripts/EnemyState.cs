using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public enum EnemyStateMachine
    {
        Idle,
        Perseguir,
        Atacar,
        Morir
    }
    private EnemyStateMachine currentState;
    public Transform jugador;
    public float rangoAtaque = 2f;
    public float rangoDeteccion = 5f;
    public float velocidad = 3f;
    private bool isDead = false;
    public float tiempoEntreAtaques = 1.5f;
    private float tiempoAtaqueActual = 0f;
    public float salud = 100f;
    void Start()
    {
        currentState = EnemyStateMachine.Idle;
    }
    void Update()
    {
        switch (currentState)
        {
            case EnemyStateMachine.Idle:
                Idle();
                break;
            case EnemyStateMachine.Perseguir:
                Perseguir();
                break;
            case EnemyStateMachine.Atacar:
                Atacar();
                break;
            case EnemyStateMachine.Morir:
                Morir();
                break;
        }
    }
    void Idle()
    {
        Debug.Log("Enemigo en estado Idle");
        if (Vector3.Distance(transform.position, jugador.position) < rangoDeteccion && !isDead)
        {
            currentState = EnemyStateMachine.Perseguir;
        }
    }
    void Perseguir()
    {
        Debug.Log("Enemigo persiguiendo al jugador");
        transform.position = Vector3.MoveTowards(transform.position, jugador.position, velocidad * Time.deltaTime);
        if (Vector3.Distance(transform.position, jugador.position) < rangoAtaque)
        {
            currentState = EnemyStateMachine.Atacar;
        }
        if (Vector3.Distance(transform.position, jugador.position) > rangoDeteccion)
        {
            currentState = EnemyStateMachine.Idle;
        }
    }

    void Atacar()
    {
        Debug.Log("Enemigo atacando...");
        tiempoAtaqueActual += Time.deltaTime;

        if (tiempoAtaqueActual >= tiempoEntreAtaques)
        {

            jugador.GetComponent<PlayerHealth>().RecibirDaño(10);

            tiempoAtaqueActual = 0f;
        }

        if (Vector3.Distance(transform.position, jugador.position) > rangoAtaque)
        {
            currentState = EnemyStateMachine.Perseguir;
        }

    }
    void Morir()
    {
        Debug.Log("Logica de enemigo murio ");
        isDead = true;

        Destroy(gameObject);
    }
    

    public void RecibirDaño(float cantidad)
    {
        if (isDead) return;

        salud -= cantidad;
        if (salud <= 0)
        {
            currentState = EnemyStateMachine.Morir;
        }
    }
}
