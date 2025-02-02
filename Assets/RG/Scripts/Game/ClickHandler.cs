using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickHandler : MonoBehaviour
{
    private Tile MyTile;

    [SerializeField] private UnityEvent OnClickEvent;

    private void Start()
    {
        MyTile = GetComponent<Tile>();
    }

    private void OnMouseDown()
    {
        MyTile.DestroyBlock();
    }
}
