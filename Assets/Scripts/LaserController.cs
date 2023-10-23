using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypesLaser
{
    LASER_UP,
    LASER_LEFT,
    LASER_RIGHT,
    LASER_DOWN
}
public class LaserController : MonoBehaviour
{
    [SerializeField] private Transform objetLaser;
    [SerializeField] private Transform objetLaser1;
    [SerializeField] private float damage;
    private float distance;
    [SerializeField] private TypesLaser category;
    [SerializeField] private bool rotateLaser;
    [SerializeField] private float speedRotate;

    private Vector2 startPosition;
    private Vector2 direction;

    private LineRenderer lr;
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Laser();
    }

    private void Laser()
    {
        switch (category)
        {
            case TypesLaser.LASER_UP: 
                direction = objetLaser.transform.up; 
                break;
            case TypesLaser.LASER_DOWN:
                direction = objetLaser.transform.up * -1; 
                break;
            case TypesLaser.LASER_RIGHT: 
                direction = objetLaser.transform.right; 
                break;
            case TypesLaser.LASER_LEFT: 
                direction = objetLaser.transform.right * -1; 
                break;
        }

        if (rotateLaser)
        {
            transform.Rotate(new Vector3(0f, 0f, speedRotate) * Time.deltaTime);
        }

        distance = CalculateDistance();

        startPosition = objetLaser.transform.position;

        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, distance);

        lr.SetPosition(0, objetLaser.transform.position);
        if (hit)
        {
            if (hit.transform.tag == "Player")
            {
                
                float playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().health;
                if (playerLife > 4f)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().ReceiveDamage(damage);
                }
                lr.SetPosition(1, hit.point);
            }
            else
            {
                
                lr.SetPosition(1, hit.point);
            }
        }
        else
        {
            lr.SetPosition(1, objetLaser1.transform.position);
        }

    }

    private float CalculateDistance()
    {
        Vector2 obj = new Vector2(objetLaser.transform.position.x, objetLaser.transform.position.y);
        Vector2 obj1 = new Vector2(objetLaser1.transform.position.x, objetLaser1.transform.position.y);

        return Vector2.Distance(obj, obj1);
    }
}
