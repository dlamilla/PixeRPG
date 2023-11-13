using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SombraScript : MonoBehaviour
{
    public Transform target; // Debes asignar el jugador como el objetivo en el Inspector
    public float velocidad = 5.0f; // Ajusta la velocidad según tus necesidades

    void Update()
    {
        if (target != null)
        {
            // Mueve la sombra hacia la posición del jugador
            transform.position = Vector3.Lerp(transform.position, target.position, velocidad * Time.deltaTime);
        }
    }

    // Función para establecer el objetivo de la sombra
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
