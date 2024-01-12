using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTransition : MonoBehaviour
{
    private Animator _anim;
    [SerializeField] private AnimationClip _animacionFinal;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimationTransition() 
    {
        StartCoroutine(TimeToChange());
    }

    IEnumerator TimeToChange()
    {
        _anim.SetTrigger("Iniciar");

        
        yield return new WaitForSeconds(_animacionFinal.length);

        _anim.SetBool("Fin", true);
    }
}
