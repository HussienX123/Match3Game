using Hussien;
using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    [Header("Position")]

    [SerializeField] private int Row;

    [SerializeField] private int Column;


    [Header("Item Manager")]

    [Tooltip("if you want random block leave it at -1 , or choose specific block by typing its index")]
    [SerializeField] private int CurrentBlockIndex = -1;

    [Tooltip("the child items not the items in the prefab folder")]
    [SerializeField] private Item[] Items;

    private MeshRenderer _MeshRenderer;

    #region Item Generation
    private void Start()
    {
        if (CurrentBlockIndex == -1)
        {
            SetRandomBlock();
        }
        else
        {
            SetBlock();
        }

        _MeshRenderer = GetComponent<MeshRenderer>();
    }

    public void SetBlock()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].gameObject.SetActive(false);
        }

        Items[CurrentBlockIndex].gameObject.SetActive(true);
    }

    public void SetRandomBlock()
    {
        CurrentBlockIndex = Random.Range(0, Items.Length);
        for (int i = 0; i < Items.Length; i++)
        {
            if (i == CurrentBlockIndex)
            {
                Items[i].gameObject.SetActive(true);
            }
            else
            {
                Items[i].gameObject.SetActive(false);
            }
        }
    }
    #endregion

    #region Grid Position
    public void SetPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }
    #endregion

    #region Click Gameplay
    public void DestroyBlock()
    {
        Tile UpperTile = GridManager.Instance.GetUpperTile(Row, Column);

        Items[CurrentBlockIndex].gameObject.SetActive(false);;

        if (UpperTile == null) //this is the top row
        {
            CurrentBlockIndex = Random.Range(0,Items.Length);
            SetBlock();
        } 
        else //this is any row except the top row
        {
            UpperTile.FallBlock(Row , Column);
        }

        StartCoroutine(ChangeColor());
    }

    public void FallBlock(int _Row, int _Column)
    {
        if (Row == 0) //this is the bottom row
        {
            return;
        }

        Tile UpperTile = GridManager.Instance.GetUpperTile(_Row, _Column);
        Tile CurrentBlock = GridManager.Instance.GetTile(_Row, _Column);

        CurrentBlock.CurrentBlockIndex = GridManager.Instance.IsMaxRow(_Row)? Random.Range(0,Items.Length) : CurrentBlockIndex;

        CurrentBlock.SetBlock();

        if (!GridManager.Instance.IsMaxRow(_Row))
        {
            if (UpperTile != null)
            {
                UpperTile.FallBlock(Row, Column);
            }
        }

        HorizontalCheck();
    }

    public void HorizontalCheck()
    {
        Tile LeftBlock = GridManager.Instance.GetTile(Row, Column - 1);
        Tile RightBlock = GridManager.Instance.GetTile(Row, Column + 1);
        Tile LeftLeftBlock = GridManager.Instance.GetTile(Row, Column - 2);
        Tile RightRightBlock = GridManager.Instance.GetTile(Row, Column + 2);

        if (LeftBlock != null && RightBlock != null)
        {
            if (LeftBlock.CurrentBlockIndex == CurrentBlockIndex && RightBlock.CurrentBlockIndex == CurrentBlockIndex)
            {
                LeftBlock.DestroyBlock();
                RightBlock.DestroyBlock();
                DestroyBlock();
            }
        }

        if (LeftLeftBlock != null && LeftBlock != null)
        {
            if (LeftLeftBlock.CurrentBlockIndex == CurrentBlockIndex && LeftBlock.CurrentBlockIndex == CurrentBlockIndex)
            {
                LeftLeftBlock.DestroyBlock();
                LeftBlock.DestroyBlock();
                DestroyBlock();
            }
        }

        if (RightRightBlock != null && RightBlock != null)
        {
            if (RightRightBlock.CurrentBlockIndex == CurrentBlockIndex && RightBlock.CurrentBlockIndex == CurrentBlockIndex)
            {
                RightRightBlock.DestroyBlock();
                RightBlock.DestroyBlock();
                DestroyBlock();
            }
        }

    }

    IEnumerator ChangeColor()
    {
        _MeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        _MeshRenderer.material.color = Color.white;
    }
    #endregion

}
