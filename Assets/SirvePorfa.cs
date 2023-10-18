using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirvePorfa : MonoBehaviour
{
    private Animator enemyAnimator;

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
    }

    public void DisablePlayerSpriteRenderer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.enabled = false;
        }
    }

    public void EnablePlayerSpriteRenderer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        if (playerSpriteRenderer != null)
        {
            playerSpriteRenderer.enabled = true;
        }
    }
}
