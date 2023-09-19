using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeSpawn
{
    BABYRAT
}

[RequireComponent(typeof(ChangeAnimation))]
public class SpawnCustom : MonoBehaviour
{
    [SerializeField] private TypeSpawn category;
    [SerializeField] private float hpEnemy;
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private Transform player;
    private ChangeAnimation changeDirections;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        changeDirections = GetComponent<ChangeAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpEnemy > 0)
        {
            switch (category)
            {
                case TypeSpawn.BABYRAT:
                    transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    Vector2 temp = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                    changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                    break;
            }
        }
    }
}
