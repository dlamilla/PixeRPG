using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSign : MonoBehaviour
{
    [SerializeField] private float levelPlayer;
    [SerializeField] private GameObject sign;
    [SerializeField] private GameObject signOrObject;
    [SerializeField] private GameObject tree;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        levelPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().level;
        if (levelPlayer >= 4)
        {
            sign.SetActive(true);
            signOrObject.SetActive(false);
            tree.SetActive(false);
        }
        else
        {
            signOrObject.SetActive(true);
        }
    }
}
