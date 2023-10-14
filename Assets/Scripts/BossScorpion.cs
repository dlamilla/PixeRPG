using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScorpion : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        changeDirections = GetComponent<ChangeAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = (player.position - transform.position).normalized;
        rb.velocity = moveDirection * speed;

        changeDirections.changeAnim(moveDirection);

        anim.SetBool("isWalking", true);
    }
}
