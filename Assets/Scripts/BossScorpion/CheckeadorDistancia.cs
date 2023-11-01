using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckeadorDistancia : MonoBehaviour
{
    public Animator enemyAnimator;
    public Collider2D squareCollider;
    public Transform player;

    private bool jugadorDentroDelSquare = false;
    private bool triggerActivado = false;

    void Update()
    {
        // Comprobar si el jugador está dentro del square
        jugadorDentroDelSquare = squareCollider.OverlapPoint(player.position);

        // Si el jugador está fuera del square y el trigger no está activado, activamos el trigger y marcamos que está activado.
        if (!jugadorDentroDelSquare && !triggerActivado)
        {
            enemyAnimator.SetTrigger("Disparando");
            triggerActivado = true;
        }

        // Si el jugador está dentro del square, desactivamos el trigger y marcamos que no está activado.
        if (jugadorDentroDelSquare && triggerActivado)
        {
            enemyAnimator.ResetTrigger("Disparando");
            triggerActivado = false;
        }
    }
}
