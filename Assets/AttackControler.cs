using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackControler : MonoBehaviour
{
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        // Obt�n la referencia al componente Animator de tu personaje
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Verifica si el jugador presiona una tecla o bot�n para iniciar el ataque
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            // Determina la direcci�n del ataque bas�ndote en las teclas presionadas
            Vector2 attackDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Llama a la funci�n de ataque con la direcci�n del ataque
            Attack(attackDirection);
        }
    }

    void Attack(Vector2 attackDirection)
    {
        // Verifica si la direcci�n del ataque es predominante en horizontal o vertical
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

        // Establecer una bandera para evitar m�ltiples ataques al mismo tiempo
        isAttacking = true;

        // Restablecer la bandera despu�s de que termine la animaci�n de ataque
        Invoke("ResetAttack", 0.5f); // Ajusta el tiempo seg�n la duraci�n de tu animaci�n de ataque
    }

    void ResetAttack()
    {
        isAttacking = false;
    }
}


