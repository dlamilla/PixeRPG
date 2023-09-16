using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControler : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        // Obtén la referencia al componente Animator de tu personaje
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica si el jugador presiona una tecla o botón para iniciar el ataque
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            // Determina la dirección del ataque basándote en las teclas presionadas
            Vector2 attackDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Llama a la función de ataque con la dirección del ataque
            Attack(attackDirection);
        }
    }

    void Attack(Vector2 attackDirection)
    {
        // Verifica si la dirección del ataque es predominante en horizontal o vertical
        if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
        {
            // Ataque horizontal
            animator.SetTrigger(attackDirection.x > 0 ? "AttackRight" : "AttackLeft");
        }
        else
        {
            // Ataque vertical
            animator.SetTrigger(attackDirection.y > 0 ? "AttackUp" : "AttackDown");
        }

        // Establecer una bandera para evitar múltiples ataques al mismo tiempo
        isAttacking = true;

        // Restablecer la bandera después de que termine la animación de ataque
        Invoke("ResetAttack", 0.5f); // Ajusta el tiempo según la duración de tu animación de ataque
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}


