using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrawMove : MonoBehaviour
{
    public enum TypeDirection
    {
        RIGHT,
        LEFT
    }

    [SerializeField] private TypeDirection category;
    [SerializeField] private float speed;
    [SerializeField] private float timeLife;
    [SerializeField] private float damage;
    private SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch (category)
        {
            case TypeDirection.RIGHT:
                sp.flipX = true;
                transform.position += (Vector3.right).normalized * speed * Time.fixedDeltaTime;
                break;
            case TypeDirection.LEFT:
                sp.flipX = false;
                transform.position += (Vector3.left).normalized * speed * Time.fixedDeltaTime;
                break;
        }
        StartCoroutine(SpawnStraw());
    }

    private IEnumerator SpawnStraw()
    {
        yield return new WaitForSeconds(timeLife);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            float playerLife = other.GetComponent<Player>().health;
            if (playerLife > 0)
            {
                Debug.Log("Daño arbusto");
                other.gameObject.GetComponent<Player>().ReceiveDamage(damage);
            }
        }
    }
}
