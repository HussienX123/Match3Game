using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEditor;
using UnityEngine.UIElements;
using System;

public struct tiIe
{
    public Tile[] tile;
}

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

        [SerializeField] tiIe[] tilex;

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

        #region Editor Events
        public void BuildGrid()
        {

            if (transform.childCount > 0)
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
                    Tile newTile = Instantiate(TilePrefab, new Vector3(j * (ColumnSpacing + 1), i * (RowSpacing + 1), 0), Quaternion.identity);
                    newTile.transform.SetParent(transform);
                    newTile.name = "Tile " + i + " " + j;
                    newTile.SetPosition(i, j);
                    Tiles.Add(newTile);
                }
            }

            //focus camera on center
            float gridWidth = (Column - 1) * (ColumnSpacing + 1);
            float gridHeight = (Row - 1) * (RowSpacing + 1);
            Camera.main.transform.position = new Vector3(gridWidth / 2, gridHeight / 2, -Row * (RowSpacing + 1));
        }

        public void RandomizeBlocks()
        {
            for (int i = 0; i < Tiles.Count; i++)
            {
                if (Tiles[i] != null)
                    Tiles[i].SetRandomBlock();  
            }
        }
        #endregion

        #region Gameplay Tile Events
        public Tile GetUpperTile(int _Row, int _Column)
        {
            if (_Row == Row - 1) //the is the top row
                return null;

            int index = (_Row + 1) * Column + _Column;
            return Tiles[index];
        }

        public Tile GetTile(int _Row, int _Column)
        {
            int index = _Row * Column + _Column;

            if (index < 0 || index >= Tiles.Count)
            {
                return null;
            }

            return Tiles[index];
        }

        public bool IsMaxRow(int _Row)
        {
            return _Row == (Row - 1);
        }
        #endregion

        #region validation
        private void OnValidate()
        {
        #if UNITY_EDITOR
            Debug.LogWarning("<color=red>GridManager: Make sure if you change anything in the prefab you must rebuild your grid</color>");
        #endif
        }
        #endregion
    }
}
