using Hussien;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Position")]

    [SerializeField] private int Row;

    [SerializeField] private int Column;


    [Header("Item Manager")]

    [Tooltip("if you want random block leave it at -1 , or choose specific block by typing its index")]
    public int CurrentBlockIndex = -1;

    [Tooltip("the child items not the items in the prefab folder")]
    [SerializeField] private Item[] Items;

    private MeshRenderer _MeshRenderer;

    private AnimationController AnimationController;

    private bool IsFalling = false;

    #region Item Generation
    private void Start()
    {
        if (CurrentBlockIndex == -1)
        {
            Tile PreviousTile = GridManager.Instance.GetUpperTile(Row, Column - 1);
            SetRandomBlock(PreviousTile);
        }
        else
        {
            SetBlock();
        }

        _MeshRenderer = GetComponent<MeshRenderer>();
        AnimationController = GetComponent<AnimationController>();
    }

    public void SetBlock()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].gameObject.SetActive(false);
        }

        Items[CurrentBlockIndex].gameObject.SetActive(true);
    }

    public void SetRandomBlock(Tile PreviousTile)
    {
        CurrentBlockIndex = Random.Range(0, Items.Length);

        if (PreviousTile != null)
        {
            while (PreviousTile.CurrentBlockIndex == CurrentBlockIndex)
            {
                CurrentBlockIndex = Random.Range(0, Items.Length);
            }
        }

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

    #region Position
    public void SetPosition(int row, int column)
    {
        Row = row;
        Column = column;
    }
    #endregion

    #region Click Gameplay
    public void DestroyBlock()
    {
        if (IsFalling)
        {
            return;
        }

        Tile UpperTile = GridManager.Instance.GetUpperTile(Row, Column);

        Items[CurrentBlockIndex].gameObject.SetActive(false);

        if (UpperTile == null) //this is the top row
        {
            CurrentBlockIndex = Random.Range(0, Items.Length);
            SetBlock();
        } 
        else //this is any row except the top row
        {
            StartCoroutine(UpperTile.FallBlock(Row, Column));
        }

        GridManager.Instance.QueueHorizontalCheck(Row);

        StartCoroutine(ChangeColor());
    }

    IEnumerator FallBlock(int _Row, int _Column)
    {
        IsFalling = true;

        if (Row == 0) //this is the bottom row
        {
            yield return null;
        }

        Tile UpperTile = GridManager.Instance.GetUpperTile(_Row, _Column);
        Tile CurrentTile = GridManager.Instance.GetTile(_Row, _Column);

        CurrentTile.CurrentBlockIndex = GridManager.Instance.IsMaxRow(_Row) ? Random.Range(0, Items.Length) : CurrentBlockIndex;


        if (!GridManager.Instance.IsMaxRow(_Row))
        {
            if (UpperTile != null)
            {
                StartCoroutine(UpperTile.FallBlock(Row, Column));
                AnimationController.MoveItemAnimation(CurrentTile.AnimationController.GetItemPosition());
            }
        }

        yield return new WaitUntil(() => AnimationController.MoveAnimation == false);

        CurrentTile.SetBlock();

        GridManager.Instance.QueueHorizontalCheck(Row);

        IsFalling = false;
    }

    IEnumerator ChangeColor()
    {
        _MeshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.5f);

        _MeshRenderer.material.color = Color.white;
    }
    #endregion

    #region Validation
    [ExecuteInEditMode]
    private void OnValidate()
    {
#if UNITY_EDITOR
        if(CurrentBlockIndex >= Items.Length)
        {
            CurrentBlockIndex = 0;
            Debug.LogError("CurrentBlockIndex is out of range");
        }

        if(CurrentBlockIndex > 0 && CurrentBlockIndex < Items.Length)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].gameObject.SetActive(false);
            }

            Items[CurrentBlockIndex].gameObject.SetActive(true);
        }
#endif
    }
    #endregion
}
