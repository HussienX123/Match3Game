using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Hussien
{
    [ExecuteInEditMode]
    public class GridManager : MonoBehaviour
    {
        [Header("Grid Setup")]

        [SerializeField] private int Row = 0;

        [SerializeField] private int Column = 0;

        [SerializeField] Tile TilePrefab;

        [SerializeField] List<Tile> Tiles = new List<Tile>();


        [Header("Spacing Settings")]

        [SerializeField] private float RowSpacing = 0f;

        [SerializeField] private float ColumnSpacing = 0f;



        #region Singleton
        public static GridManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }
        #endregion

        public void BuildGrid()
        {
            if(transform.childCount > 0)
            {
                for (int i = 0; i < Tiles.Count; i++)
                {
                    if (Tiles[i] != null)
                        DestroyImmediate(Tiles[i].gameObject);
                }
                Tiles.Clear();
            }


            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Column; j++)
                {
                    Tile NewTile = Instantiate(TilePrefab, new Vector3(j * (ColumnSpacing + 1), i * (RowSpacing + 1), 0), Quaternion.identity);
                    NewTile.transform.SetParent(transform);
                    NewTile.Row = i;
                    NewTile.Column = j;
                    Tiles.Add(NewTile);
                }
            }

            //focus camera on center
            float gridWidth = (Column - 1) * (ColumnSpacing + 1);
            float gridHeight = (Row - 1) * (RowSpacing + 1);
            Camera.main.transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -Row);
        }

    }
}
