using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncaTeleport : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 3.1f;
    public float teleportTime = 45f;
    public float teleportRange = 5f;
    private Animator animator; 

    private bool isBacking = false; 
    private bool isFollowingPlayer = true; 

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponent<Animator>();
        InvokeRepeating("Teleport", teleportTime, teleportTime);
    }

    private void Update()
    {
        Vector2 directionToPlayer = (target.position - transform.position).normalized;

        if (isFollowingPlayer && !animator.GetBool("isTeleport") && !isBacking)
        {
            Vector2 newPosition = new Vector2(transform.position.x, transform.position.y) + directionToPlayer * moveSpeed * Time.deltaTime;
            transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    private void Teleport()
    {
        animator.SetBool("isTeleport", true);
        StartCoroutine(CompleteTeleportAnimation());
    }

    private IEnumerator CompleteTeleportAnimation()
    {
      
        yield return new WaitForSeconds(1.5f);

        Vector2 randomOffset = Random.insideUnitCircle.normalized * teleportRange;
        Vector3 teleportPosition = new Vector3(target.position.x + randomOffset.x, target.position.y + randomOffset.y, transform.position.z);
        transform.position = teleportPosition;

        animator.SetBool("isTeleport", false);

        animator.SetBool("isBacking", true);

        isFollowingPlayer = false;

        yield return new WaitForSeconds(1.15f);

        animator.SetBool("isBacking", false);

        isFollowingPlayer = true;
    }
}
