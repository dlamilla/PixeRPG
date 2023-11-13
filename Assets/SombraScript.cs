using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SombraScript : MonoBehaviour
{
    public Transform target; // Debes asignar el jugador como el objetivo en el Inspector
    public float velocidad = 5.0f; // Ajusta la velocidad seg�n tus necesidades

    void Update()
    {
        if (target != null)
        {
            // Mueve la sombra hacia la posici�n del jugador
            transform.position = Vector3.Lerp(transform.position, target.position, velocidad * Time.deltaTime);
        }
    }

    // Funci�n para establecer el objetivo de la sombra
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
