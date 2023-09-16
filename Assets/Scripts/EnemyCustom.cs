using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeEnemys{
    ENEMY_SPIDER,
    BOSS_1
}
public class EnemyCustom : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float speed;
    [SerializeField] private TypeEnemys category;
    [SerializeField] private float radiusSearch;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (category)
        {
            case TypeEnemys.ENEMY_SPIDER: 
                break;
            case TypeEnemys.BOSS_1: 
                //Seguir al jugador
                //Atacar
                //Vida en la mitad ataque 2
                //Spawnea
                //Muere dar exp
                //Random monedas
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusSearch);
    }
}
