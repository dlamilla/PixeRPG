using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TestPeroBola : MonoBehaviour
{
    public Transform[] teleportPoints;
    public float teleportTime = 45f;
    private Transform target;
    private Animator animator;
    private bool isFollowingPlayer = true;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        InvokeRepeating("Teleport", teleportTime, teleportTime);
    }

    void Update()
    {
        if (isFollowingPlayer && !animator.GetBool("isTeleport"))
        {
            Vector2 directionToPlayer = (target.position - transform.position).normalized;
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);

            // Aquí puedes implementar tu lógica de seguimiento o retroceso si es necesario
        }
    }

    void Teleport()
    {
        animator.SetBool("isTeleport", true);
        StartCoroutine(CompleteTeleportAnimation());
    }

    IEnumerator CompleteTeleportAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        int randomIndex = Random.Range(0, teleportPoints.Length);
        Vector3 teleportPosition = teleportPoints[randomIndex].position;

        transform.position = teleportPosition;

        animator.SetBool("isTeleport", false);
        isFollowingPlayer = false;

        yield return new WaitForSeconds(1.15f);

        isFollowingPlayer = true;
    }
}
