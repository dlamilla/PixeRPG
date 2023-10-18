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
        // Comprobar si el jugador est� dentro del square
        jugadorDentroDelSquare = squareCollider.OverlapPoint(player.position);

        // Si el jugador est� fuera del square y el trigger no est� activado, activamos el trigger y marcamos que est� activado.
        if (!jugadorDentroDelSquare && !triggerActivado)
        {
            enemyAnimator.SetTrigger("Disparando");
            triggerActivado = true;
        }

        // Si el jugador est� dentro del square, desactivamos el trigger y marcamos que no est� activado.
        if (jugadorDentroDelSquare && triggerActivado)
        {
            enemyAnimator.ResetTrigger("Disparando");
            triggerActivado = false;
        }
    }
}
