using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeNpc
{
    NPC_PATROL
}

[RequireComponent(typeof(ChangeAnimation))]
public class NpcCustom : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private TypeNpc category;
    private int currentPoint;

    private ChangeAnimation changeDirections;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        changeDirections = GetComponent<ChangeAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (category)
        {
            case TypeNpc.NPC_PATROL:
                if (transform.position != waypoints[currentPoint].transform.position)
                {
                    transform.position = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, speed * Time.deltaTime);
                    Vector2 temp = Vector2.MoveTowards(transform.position, waypoints[currentPoint].transform.position, speed * Time.deltaTime);
                    changeDirections.changeAnim(temp - new Vector2(transform.position.x, transform.position.y));
                }
                else
                {
                    ChangeGoal();
                }
                break;
        }
    }

    private void ChangeGoal()
    {
        currentPoint++;
        if (currentPoint >= waypoints.Length)
        {
            currentPoint = 0;
        }
    }
}
