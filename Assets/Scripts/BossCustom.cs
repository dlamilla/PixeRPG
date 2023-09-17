using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeBoss
{
    BOSS_NIVEL1,
    BOSS_NIVEL2
}

[RequireComponent(typeof(ChangeAnimation))]
public class BossCustom : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float hpBoss;
    [SerializeField] private float speed;
    [SerializeField] private float hitDamage;
    [SerializeField] private float timeForHit;
    [SerializeField] private TypeBoss category;
    private ChangeAnimation changeDirections;
    private Animator anim;
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player").transform;
        changeDirections = GetComponent<ChangeAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpBoss > 0)
        {
            switch (category)
            {
                case TypeBoss.BOSS_NIVEL1: 
                    break;
            }
        }
    }
}
