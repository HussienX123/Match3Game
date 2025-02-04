using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Transform ItemHolder;

    public bool MoveAnimation = false;

    Vector3 TargetTilePosition;
    Vector3 StartTilePosition;

    private void Start()
    {
        StartTilePosition = ItemHolder.position;
    }

    public void MoveItemAnimation(Vector3 Target)
    {
        TargetTilePosition = Target;
        MoveAnimation = true;
    }
    
    public Vector3 GetItemPosition()
    {
        return StartTilePosition;
    }

    private void Update()
    {
        if (MoveAnimation)
        {
            ItemHolder.position = Vector3.MoveTowards(ItemHolder.position, TargetTilePosition, 12 * Time.deltaTime);
            if (ItemHolder.position == TargetTilePosition)
            {
                ItemHolder.position = StartTilePosition;
                MoveAnimation = false;
            }
        }
    }
}
