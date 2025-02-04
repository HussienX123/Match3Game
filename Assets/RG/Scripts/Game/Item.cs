using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Animator AnimController;

    private void Start()
    {
        AnimController = GetComponent<Animator>();
    }

    public void PlayDestroyAnimation()
    {

    }
}
