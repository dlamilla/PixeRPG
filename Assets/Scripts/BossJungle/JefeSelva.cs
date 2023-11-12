using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class JefeSelva : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpEnemy;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float increasedSpeed;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private float currentSpeed;
    private Rigidbody2D rb;
    private Transform player;
    private BoxCollider2D player1;
    private bool hasLanzaGoAnimationPlayed = false;
    private bool isLanzaGoResetting = false;

    [Header("AtaqueLanza")]
    private Transform lanza;
    private bool isFollowingPlayer = true;
    private bool isLanzaDetached = false;
    private float detachTime = 5.0f;
    private float returnToLanzaTime = 5.0f;
    private float detachStartTime = -1f;
    private Vector2 lanzaPosition;
    private Transform jefeSelva;

    public enum TipoAtaque
    {
        LanzaAttack,
        SaltoAttack
    }

    [SerializeField]
    private TipoAtaque listaAtaque;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        lanza = transform.Find("Lanza");
        player1 = GameObject.FindWithTag("Player").GetComponent<BoxCollider2D>();
        changeDirections = GetComponent<ChangeAnimation>();
        jefeSelva = GameObject.FindWithTag("JefeSelva").transform;
        currentSpeed = normalSpeed;
        listaAtaque = TipoAtaque.LanzaAttack;
    }

    void Update()
    {
        switch (listaAtaque)
        {
            case TipoAtaque.LanzaAttack:
                UpdateLanzaAttack();
            break;

            case TipoAtaque.SaltoAttack:

            break;

        }
    }

    void LateUpdate()
    {
        switch (listaAtaque)
        {
            case TipoAtaque.LanzaAttack:
                LateUpdateLanzaAttack();
            break;

            case TipoAtaque.SaltoAttack:

            break;

        }
    }

    private void UpdateLanzaAttack()
    {
        if (isFollowingPlayer)
        {
            Vector2 moveDirection = (player.position - transform.position).normalized;

            changeDirections.changeAnim(moveDirection);
            anim.SetBool("enCamino", true);

            transform.position = Vector2.MoveTowards(transform.position, player.position, currentSpeed * Time.deltaTime);
        }
        else if (isLanzaDetached)
        {
            transform.position = Vector2.MoveTowards(transform.position, lanzaPosition, increasedSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, lanzaPosition) < 0.1f)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isFollowingPlayer = true;
                Debug.Log("SeguirPlayer");
                lanza.parent = jefeSelva;
                currentSpeed = normalSpeed;
                listaAtaque= TipoAtaque.LanzaAttack;
            }
        }
    }

    private void LateUpdateLanzaAttack()
    {
        if (!isLanzaDetached && Time.time >= detachTime)
        {
            if (!hasLanzaGoAnimationPlayed)
            {
                anim.SetBool("lanzaGo", true);
                hasLanzaGoAnimationPlayed = true;
                StartCoroutine(ResetLanzaGoAfterDelay());
            }

            GetComponent<SpriteRenderer>().color = Color.red;
            lanza.parent = null;
            isLanzaDetached = true;
            lanzaPosition = lanza.position;
            detachStartTime = Time.time;
            currentSpeed = increasedSpeed;
            listaAtaque= TipoAtaque.LanzaAttack;
        }
        else if (isLanzaDetached && isFollowingPlayer && Time.time >= detachStartTime + returnToLanzaTime)
        {
            isFollowingPlayer = false;
        }

        if (lanza.parent == jefeSelva)
        {
            isFollowingPlayer = true;
            listaAtaque= TipoAtaque.LanzaAttack;
        }
    }

    private IEnumerator ResetLanzaGoAfterDelay()
    {
        yield return new WaitForSeconds(0.85f);

        if (!isLanzaGoResetting)
        {
            anim.SetBool("lanzaGo", false);
            hasLanzaGoAnimationPlayed = false;
        }
    }

    public void CancelResetLanzaGo()
    {
        isLanzaGoResetting = true;
    }
}
