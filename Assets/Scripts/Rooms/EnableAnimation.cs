using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAnimation : MonoBehaviour
{
    [SerializeField] private AnimationClip _clipAnim;
    private void OnEnable()
    {
        StartCoroutine(TimeAnimation());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimeAnimation()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(_clipAnim.length);
        gameObject.SetActive(false);

    }
}
