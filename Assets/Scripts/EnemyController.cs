using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Animator enemyAnimator; 
    public Collider2D squareCollider;
    public Transform player;
    public Transform enemy;
    public float offsetDistance = 1.0f;  // Aca puedo ajustar la distancia entre el square de deteccion y el enemigo
    internal bool isSupressing;
    private bool jugadorDentroDelSquare = false;

    void Update()
    {
        // Para comprobar si el jugador está dentro del square y su collider 
        jugadorDentroDelSquare = squareCollider.OverlapPoint(player.position);

        // Si mi jugador est adentro del square y el trigger "Agarre" no está activado, entonces llamamos a activar el trigger.
        if (jugadorDentroDelSquare && !enemyAnimator.GetBool("Agarre"))
        {
            enemyAnimator.SetTrigger("Agarre");
        }

        // En caso mi jugador no esta dentro del square de deteccion, se va desactivar el booleano "Agarrando".
        if (!jugadorDentroDelSquare)
        {
            enemyAnimator.SetBool("Agarrando", false);
        }

        // Para este caso, suponemos que mi jugador está dentro del square y el trigger "Agarre" está activado, en todo caso se va activar el booleano "Agarrando".
        if (jugadorDentroDelSquare && enemyAnimator.GetBool("Agarre"))
        {
            enemyAnimator.SetBool("Agarrando", true);
        }

        // Acá llamo a las direccion de mi enemigo dependiendo su floats como direccionales
        float direccionX = enemyAnimator.GetFloat("X");
        float direccionY = enemyAnimator.GetFloat("Y");

        Vector3 nuevaPosicion = enemy.position + new Vector3(direccionX, direccionY, 0) * offsetDistance;

        squareCollider.transform.position = nuevaPosicion;
    }
}
