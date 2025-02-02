using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Animator _Animator;

    private void Start()
    {
        _Animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(_Animator != null)
            _Animator.Play("BlockPOP");
    }

}
